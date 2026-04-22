using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    private readonly ICurrentUserService _currentUserService;

    public AuthController(IAuthService authService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
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

    [Authorize]
    [HttpPost("swap-role")]
    public async Task<ActionResult<AuthResponseDto>> SwapRole(SwapRoleDto request)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _authService.SwapRoleAsync(userId, request.RoleName);
        return Ok(result);
    }
}
