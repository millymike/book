using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Book.Models;

public class User
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    [JsonIgnore] public string? PasswordHash { get; set; }
}