using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryApi.Dtos;

public class BookUpdateDto
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [Required, MinLength(1)]
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  [Range(0, int.MaxValue)]
  [JsonPropertyName("year")]
  public int Year { get; set; }

  [JsonPropertyName("authorId")]
  public int AuthorId { get; set; }
}
