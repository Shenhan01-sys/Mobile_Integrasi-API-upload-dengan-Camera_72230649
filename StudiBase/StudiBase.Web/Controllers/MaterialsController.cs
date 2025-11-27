using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudiBase.Shared.Contracts.DTOs;
using StudiBase.Shared.Domain.Entities;
using StudiBase.Shared.Domain.Enums;
using StudiBase.Web.Data;
using StudiBase.Web.Services;

namespace StudiBase.Web.Controllers
{
 [ApiController]
 [Route("api/courses/{courseId:int}/materials")]
 public class MaterialsController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly IMapper _mapper;
 private readonly IFileService _files;

 public MaterialsController(AppDbContext db, IMapper mapper, IFileService files)
 {
 _db = db; _mapper = mapper; _files = files;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<CourseMaterialDto>>> GetList(int courseId, CancellationToken ct = default)
 {
 var exists = await _db.Courses.AnyAsync(c => c.Id == courseId, ct);
 if (!exists) return NotFound();
 var mats = await _db.CourseMaterials.AsNoTracking().Include(m => m.Trainer).Where(m => m.CourseId == courseId).ToListAsync(ct);
 return Ok(_mapper.Map<List<CourseMaterialDto>>(mats));
 }

 [HttpGet("{id:int}")]
 public async Task<ActionResult<CourseMaterialDto>> GetById(int courseId, int id, CancellationToken ct = default)
 {
 var mat = await _db.CourseMaterials.AsNoTracking().Include(m => m.Trainer).FirstOrDefaultAsync(m => m.CourseId == courseId && m.Id == id, ct);
 if (mat == null) return NotFound();
 return Ok(_mapper.Map<CourseMaterialDto>(mat));
 }

 // Upload new material (multipart/form-data: file + json fields)
 [HttpPost]
 [RequestSizeLimit(50_000_000)] //50 MB
 public async Task<ActionResult<CourseMaterialDto>> Upload(int courseId, [FromForm] CourseMaterialCreateDto meta, [FromForm] IFormFile file, [FromForm] string fileType, CancellationToken ct = default)
 {
 if (!ModelState.IsValid) return ValidationProblem(ModelState);
 var course = await _db.Courses.FindAsync(new object?[] { courseId }, ct);
 if (course == null) return NotFound(new ProblemDetails { Title = "Course not found" });
 var trainerOk = await _db.Trainers.AnyAsync(t => t.Id == meta.UploadedByTrainerId, ct);
 if (!trainerOk) return BadRequest(new ProblemDetails { Title = "Invalid trainer", Detail = "Uploader trainer not found" });
 if (file == null || file.Length ==0) return BadRequest(new ProblemDetails { Title = "Empty file" });

 // parse fileType enum
 if (!Enum.TryParse<FileType>(fileType, true, out var parsedType)) parsedType = FileType.txt;

 var save = await _files.SaveAsync(file, new[] { "courses", courseId.ToString(), "materials" }, ct);
 var entity = new CourseMaterial
 {
 Title = meta.Title,
 Description = meta.Description,
 FilePath = save.RelativePath,
 FileTypeMaterial = parsedType,
 FileSize = save.Size,
 UploadedOn = DateTime.UtcNow,
 UploadedByTrainerId = meta.UploadedByTrainerId,
 CourseId = courseId
 };
 _db.CourseMaterials.Add(entity);
 await _db.SaveChangesAsync(ct);
 var dto = _mapper.Map<CourseMaterialDto>(entity);
 return CreatedAtAction(nameof(GetById), new { courseId, id = entity.Id }, dto);
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int courseId, int id, CancellationToken ct = default)
 {
 var entity = await _db.CourseMaterials.FirstOrDefaultAsync(m => m.CourseId == courseId && m.Id == id, ct);
 if (entity == null) return NotFound();
 // delete physical file
 await _files.DeleteAsync(entity.FilePath);
 _db.CourseMaterials.Remove(entity);
 await _db.SaveChangesAsync(ct);
 return NoContent();
 }

 // Download file binary
 [HttpGet("{id:int}/download")]
 public async Task<IActionResult> Download(int courseId, int id)
 {
 var entity = await _db.CourseMaterials.AsNoTracking().FirstOrDefaultAsync(m => m.CourseId == courseId && m.Id == id);
 if (entity == null) return NotFound();
 if (!_files.TryGetAbsolutePath(entity.FilePath, out var abs) || !System.IO.File.Exists(abs)) return NotFound(new ProblemDetails { Title = "File missing" });
 var contentType = _files.GetContentType(abs);
 var stream = System.IO.File.OpenRead(abs);
 return File(stream, contentType, Path.GetFileName(abs));
 }
 }
}
