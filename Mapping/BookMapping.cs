using LibraryApi.Dtos;
using LibraryApi.Models;

namespace LibraryApi.Mapping;

public static class BookMapping
{
  public static BookDto ToDto(this Book b) => new()
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
