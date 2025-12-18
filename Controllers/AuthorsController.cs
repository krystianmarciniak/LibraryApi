using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers;

[ApiController]
[Route("authors")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthorsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
    {
        var authors = await _db.Authors.AsNoTracking().ToListAsync();
        return Ok(authors.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var author = await _db.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();
        return Ok(ToDto(author));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] AuthorCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var author = new Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        return Created($"/authors/{author.Id}", ToDto(author)); // 201
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();
        if (!ModelState.IsValid) return BadRequest();

        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();

        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;

        await _db.SaveChangesAsync();
        return NoContent(); // 204
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        return NoContent(); // 204
    }

    private static AuthorDto ToDto(Author a) => new()
    {
        Id = a.Id,
        FirstName = a.FirstName,
        LastName = a.LastName
    };
}
