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
    [Route("api/trainers")]
    public class TrainerController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _files;

        public TrainerController(AppDbContext db, IMapper mapper, IFileService files)
        {
            _db = db;
            _mapper = mapper;
            _files = files;
        }

        // GET: api/trainers?q=&page=1&pageSize=20
        [HttpGet]
        public async Task<ActionResult<PagedResult<TrainerDto>>> GetList([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 20 : pageSize;
            var query = _db.Trainers.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var pattern = $"%{q}%";
                query = query.Where(t => EF.Functions.Like(t.Name, pattern) || EF.Functions.Like(t.Email, pattern));
            }
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(t => t.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            var dto = _mapper.Map<List<TrainerDto>>(items);
            return Ok(new PagedResult<TrainerDto> { Total = total, Items = dto, Page = page, PageSize = pageSize });
        }

        // GET: api/trainers/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct = default)
        {
            var items = await _db.Trainers.AsNoTracking().ToListAsync(ct);
            var dto = _mapper.Map<List<TrainerDto>>(items);
            return Ok(dto);
        }

        // GET: api/trainers/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var entity = await _db.Trainers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<TrainerDto>(entity));
        }

        // POST: api/trainers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrainerCreateDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = _mapper.Map<Trainer>(dto);
            _db.Trainers.Add(entity);
            await _db.SaveChangesAsync(ct);
            var readDto = _mapper.Map<TrainerDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, readDto);
        }

        // PUT: api/trainers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TrainerUpdateDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = await _db.Trainers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        // DELETE: api/trainers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var entity = await _db.Trainers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            // delete profile picture if any
            if (!string.IsNullOrWhiteSpace(entity.ProfilePicturePath))
            {
                await _files.DeleteAsync(entity.ProfilePicturePath);
            }
            _db.Trainers.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        // POST: api/trainers/{id}/photo
        [HttpPost("{id:int}/photo")]
        public async Task<IActionResult> UploadPhoto(int id, [FromForm] IFormFile file, CancellationToken ct = default)
        {
            var entity = await _db.Trainers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return NotFound();
            if (file == null || file.Length == 0) return BadRequest(new ProblemDetails { Title = "Empty file" });
            var save = await _files.SaveAsync(file, new[] { "trainers", id.ToString(), "photos" }, ct);
            // delete old
            if (!string.IsNullOrWhiteSpace(entity.ProfilePicturePath))
            {
                await _files.DeleteAsync(entity.ProfilePicturePath);
            }
            entity.ProfilePicturePath = save.RelativePath;
            await _db.SaveChangesAsync(ct);
            return Ok(new { path = entity.ProfilePicturePath, size = save.Size, contentType = save.ContentType });
        }
    }
}
