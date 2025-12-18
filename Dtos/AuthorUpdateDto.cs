using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryApi.Dtos;

public class AuthorUpdateDto
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [Required, MinLength(1)]
  [JsonPropertyName("first_name")]
  public string FirstName { get; set; } = string.Empty;

  [Required, MinLength(1)]
  [JsonPropertyName("last_name")]
  public string LastName { get; set; } = string.Empty;
}
