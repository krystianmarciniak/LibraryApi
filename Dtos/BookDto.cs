using System.Text.Json.Serialization;

namespace LibraryApi.Dtos;

public class BookDto
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  [JsonPropertyName("year")]
  public int Year { get; set; }

  [JsonPropertyName("author")]
  public AuthorDto Author { get; set; } = new();
}
