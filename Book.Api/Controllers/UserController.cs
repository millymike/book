using Book.Api.Dtos;
using Book.Features;
using Book.Models;
using Book.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Book.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly IBookService _bookService;

    public UserController(IBookService bookService)
        {
            _bookService = bookService;
        } 

    [HttpPost]
     public async Task<ActionResult<User>> Register(RegisterUserRequest registerUserRequest)
    {
        var passwordHash = _bookService.CreatePasswordHash(registerUserRequest.Password);
        var user = await _bookService.GetUserByEmailAddress(registerUserRequest.EmailAddress);

        if (user == null && user?.Username!=registerUserRequest.Username)
        {
             var newUser = new User
            {
               EmailAddress = registerUserRequest.EmailAddress,
                Username = registerUserRequest.Username,
                PasswordHash = await passwordHash
            };
           await _bookService.CreateUser(newUser);
            return Ok(newUser);
        }

        return BadRequest("User already exist");
    }

    [HttpPost]
    public async Task<ActionResult<JwtDto>> Login(LoginUserRequest loginUserRequest)
    {
    var user = await _bookService.GetUserByEmailAddress(loginUserRequest.EmailAddress);
    if (user == null) return BadRequest("Invalid email/password");
          if (_bookService.VerifyPassword(loginUserRequest.Password, user) == false)
        {
            return BadRequest("Invalid email/password");
          }

        return Ok(new JwtDto{AccessToken = await _bookService.CreateToken(user)});
    }
}