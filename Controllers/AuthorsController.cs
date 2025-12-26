using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Mapping;

namespace LibraryApi.Controllers;

[ApiController]
[Route("authors")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthorsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        var authors = await _db.Authors.AsNoTracking().ToListAsync();
        return Ok(authors.Select(a => a.ToDto()));

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
        var author = await _db.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();
        return Ok(author.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] AuthorCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var author = new Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        return Created($"/authors/{author.Id}", author.ToDto()); 
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
        return NoContent(); 
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        return NoContent(); 
    }

   
}
