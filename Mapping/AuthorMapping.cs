using LibraryApi.Dtos;
using LibraryApi.Models;

namespace LibraryApi.Mapping;

public static class AuthorMapping
{
  public static AuthorDto ToDto(this Author a) => new()
  {
    Id = a.Id,
    FirstName = a.FirstName,
    LastName = a.LastName
  };
}
