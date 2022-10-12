using System.Text.Json.Serialization;

namespace Book.Models;

public class BookPost
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string[]? Tags { get; set; }
    [JsonIgnore] public string CategoryName { get; set; } = string.Empty;
    public Category? Category { get; set; }
    [JsonIgnore] public int AuthorId { get; set; }
    public Author? Author { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}