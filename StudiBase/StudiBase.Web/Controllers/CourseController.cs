using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudiBase.Shared.Contracts.DTOs;
using StudiBase.Shared.Contracts.Paging;
using StudiBase.Shared.Domain.Entities;
using StudiBase.Web.Data;
using StudiBase.Web.Services;

namespace StudiBase.Web.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _files;

        public CourseController(AppDbContext db, IMapper mapper, IFileService files)
        {
            _db = db;
            _mapper = mapper;
            _files = files;
        }

        // GET: api/courses?q=&page=1&pageSize=20
        [HttpGet]
        public async Task<ActionResult<PagedResult<CourseDto>>> GetList([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 20 : pageSize;
            IQueryable<Course> query = _db.Courses.AsNoTracking().Include(c => c.Trainer);
            if (!string.IsNullOrWhiteSpace(q))
            {
                var pattern = $"%{q}%";
                query = query.Where(c => EF.Functions.Like(c.Title, pattern)
                                       || (c.Description != null && EF.Functions.Like(c.Description, pattern))
                                       || (c.Category != null && EF.Functions.Like(c.Category, pattern)));
            }
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            var dto = _mapper.Map<List<CourseDto>>(items);
            return Ok(new PagedResult<CourseDto> { Total = total, Items = dto, Page = page, PageSize = pageSize });
        }

        // GET: api/courses/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct = default)
        {
            var items = await _db.Courses.AsNoTracking().Include(c => c.Trainer).ToListAsync(ct);
            var dto = _mapper.Map<List<CourseDto>>(items);
            return Ok(dto);
        }

        // GET: api/courses/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var entity = await _db.Courses.AsNoTracking().Include(c => c.Trainer).FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<CourseDto>(entity));
        }

        // POST: api/courses
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            // validasi trainer
            var trainerExists = await _db.Trainers.AnyAsync(t => t.Id == dto.TrainerId, ct);
            if (!trainerExists) return BadRequest(new ProblemDetails { Title = "Invalid TrainerId", Detail = "Trainer tidak ditemukan." });
            var entity = _mapper.Map<Course>(dto);
            _db.Courses.Add(entity);
            await _db.SaveChangesAsync(ct);
            var readDto = _mapper.Map<CourseDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, readDto);
        }

        // PUT: api/courses/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            // validasi trainer jika berubah
            if (entity.TrainerId != dto.TrainerId)
            {
                var trainerExists = await _db.Trainers.AnyAsync(t => t.Id == dto.TrainerId, ct);
                if (!trainerExists) return BadRequest(new ProblemDetails { Title = "Invalid TrainerId", Detail = "Trainer tidak ditemukan." });
            }
            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        // DELETE: api/courses/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var entity = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            // delete thumbnail if any
            if (!string.IsNullOrWhiteSpace(entity.thumbnailImagePath))
            {
                await _files.DeleteAsync(entity.thumbnailImagePath);
            }
            _db.Courses.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        // POST: api/courses/{id}/thumbnail
        [HttpPost("{id:int}/thumbnail")]
        public async Task<IActionResult> UploadThumbnail(int id, [FromForm] IFormFile file, CancellationToken ct = default)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (course == null) return NotFound();
            if (file == null || file.Length == 0) return BadRequest(new ProblemDetails { Title = "Empty file" });
            var save = await _files.SaveAsync(file, new[] { "courses", id.ToString(), "thumbnail" }, ct);
            if (!string.IsNullOrWhiteSpace(course.thumbnailImagePath))
            {
                await _files.DeleteAsync(course.thumbnailImagePath);
            }
            course.thumbnailImagePath = save.RelativePath;
            await _db.SaveChangesAsync(ct);
            return Ok(new { path = course.thumbnailImagePath, size = save.Size, contentType = save.ContentType });
        }
    }
}
