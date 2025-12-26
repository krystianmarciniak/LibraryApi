using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Mapping;

namespace LibraryApi.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
  private readonly AppDbContext _db;
  public BooksController(AppDbContext db) => _db = db;

  [HttpGet]
  public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] int? authorId)
  {
    var q = _db.Books.Include(b => b.Author).AsNoTracking();

    if (authorId.HasValue)
      q = q.Where(b => b.AuthorId == authorId.Value);

    var books = await q.ToListAsync();
    return Ok(books.Select(b => b.ToDto()));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<BookDto>> GetBook(int id)
  {
    var book = await _db.Books.Include(b => b.Author).AsNoTracking()
        .FirstOrDefaultAsync(b => b.Id == id);

    if (book is null) return NotFound();
    return Ok(book.ToDto());
  }

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
    return Created($"/books/{book.Id}", book.ToDto());
  }

  // Author is eagerly loaded to match required response shape in tests
  [HttpPut("{id:int}")]
  public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto dto)
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
    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
    if (book is null) return NotFound();

    _db.Books.Remove(book);
    await _db.SaveChangesAsync();
    return NoContent();
  }
}