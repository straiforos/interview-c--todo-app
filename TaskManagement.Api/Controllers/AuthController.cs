using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Services;
using TaskManagement.Api.Interceptors;

namespace TaskManagement.Api.Controllers;

/// <summary>
/// Controller for managing user authentication and registration.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExceptionFilter]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }
}
