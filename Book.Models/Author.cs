using System.Text.Json.Serialization;

namespace Book.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [JsonIgnore] public List<BookPost>? BookPosts { get; set; }
}