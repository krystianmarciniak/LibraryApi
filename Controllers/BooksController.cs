using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
  private readonly AppDbContext _db;
  public BooksController(AppDbContext db) => _db = db;

  // GET /books oraz GET /books?authorId=1
  [HttpGet]
  public async Task<ActionResult<IEnumerable<BookDto>>> GetAll([FromQuery] int? authorId)
  {
    var q = _db.Books.Include(b => b.Author).AsNoTracking();

    if (authorId.HasValue)
      q = q.Where(b => b.AuthorId == authorId.Value);

    var books = await q.ToListAsync();
    return Ok(books.Select(ToDto));
  }

  // GET /books/{id}
  [HttpGet("{id:int}")]
  public async Task<ActionResult<BookDto>> GetById(int id)
  {
    var book = await _db.Books.Include(b => b.Author).AsNoTracking()
        .FirstOrDefaultAsync(b => b.Id == id);

    if (book is null) return NotFound();
    return Ok(ToDto(book));
  }

  // POST /books
  [HttpPost]
  public async Task<ActionResult<BookDto>> Create([FromBody] BookCreateDto dto)
  {
    if (!ModelState.IsValid) return BadRequest();

    var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId);
    if (author is null) return BadRequest();

    var book = new Book
    {
      Title = dto.Title,
      Year = dto.Year,
      AuthorId = dto.AuthorId
    };

    _db.Books.Add(book);
    await _db.SaveChangesAsync();

    book.Author = author;
    return Created($"/books/{book.Id}", ToDto(book)); // 201
  }

  // PUT /books/{id}
  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDto dto)
  {
    if (id != dto.Id) return BadRequest();
    if (!ModelState.IsValid) return BadRequest();

    var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
    if (book is null) return NotFound();

    var authorExists = await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId);
    if (!authorExists) return BadRequest();

    book.Title = dto.Title;
    book.Year = dto.Year;
    book.AuthorId = dto.AuthorId;

    await _db.SaveChangesAsync();
    return NoContent(); // 204
  }

  // DELETE /books/{id}
  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
    if (book is null) return NotFound();

    _db.Books.Remove(book);
    await _db.SaveChangesAsync();
    return NoContent(); // 204
  }

  private static BookDto ToDto(Book b) => new()
  {
    Id = b.Id,
    Title = b.Title,
    Year = b.Year,
    Author = new AuthorDto
    {
      Id = b.Author!.Id,
      FirstName = b.Author.FirstName,
      LastName = b.Author.LastName
    }
  };
}
