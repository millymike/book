using System.Text.Json.Serialization;

namespace Book.Models;

public class Category
{
    public string CategoryName { get; set; } = string.Empty;

    [JsonIgnore] public List<BookPost>? BookPosts { get; set; }
}