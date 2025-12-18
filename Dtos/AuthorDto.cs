using System.Text.Json.Serialization;

namespace LibraryApi.Dtos;

public class AuthorDto
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("first_name")]
  public string FirstName { get; set; } = string.Empty;

  [JsonPropertyName("last_name")]
  public string LastName { get; set; } = string.Empty;
}
