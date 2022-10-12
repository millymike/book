using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos;

public class NewBookDto
{
    [Required] public int Id { get; set; }

    [Required] public string Title { get; set; } = string.Empty;

    [Required] public string Summary { get; set; } = string.Empty;

    [Required] public string Body { get; set; } = string.Empty;

    [Required] public string[]? Tags { get; set; }

    [Required] public int AuthorId { get; set; }

    [Required] public string CategoryName { get; set; } = string.Empty;

    public DateTime Created { get; init; }
    public DateTime Updated { get; init; }
}