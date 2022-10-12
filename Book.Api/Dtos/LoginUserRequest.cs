using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos;

public class LoginUserRequest
{
    [Required] [EmailAddress] public string EmailAddress { get; set; }
    [Required] public string Password { get; set; }
}