using BlogApp.DTOs;
using BlogApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        var result = await _userService.RegisterAsync(request.Username, request.Password);

        if (!result.IsAuthenticated)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _userService.LoginAsync(request.Username, request.Password);

        if (!result.IsAuthenticated)
            return Unauthorized(new { message = result.Message });

        return Ok(result);
    }
}
