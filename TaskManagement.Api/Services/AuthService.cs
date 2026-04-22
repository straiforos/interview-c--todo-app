using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Interceptors;
using TaskManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TaskManagement.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto request);
    Task<AuthResponseDto> LoginAsync(LoginDto request);
    Task<AuthResponseDto> SwapRoleAsync(string userId, string newRole);
}

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly AppDbContext _context;

    public AuthService(UserManager<User> userManager, IOptions<JwtSettings> jwtOptions, AppDbContext context)
    {
        _userManager = userManager;
        _jwtSettings = jwtOptions.Value;
        _context = context;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
    {
        var user = new User
        {
            UserName = string.IsNullOrWhiteSpace(request.UserName) ? request.Email : request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Registration failed: {errors}");
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, "User");

        return await LoginAsync(new LoginDto { Email = request.Email, Password = request.Password });
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new ForbiddenException("Invalid credentials");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        if (string.IsNullOrEmpty(role))
        {
            await _userManager.AddToRoleAsync(user, "User");
            role = "User";
        }

        var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);
        var permissions = new List<string>();
        if (roleEntity != null)
        {
            permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleEntity.Id)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }

        var token = GenerateJwtToken(user, role, permissions);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserId = user.Id,
            Role = role,
            Permissions = permissions
        };
    }

    public async Task<AuthResponseDto> SwapRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found");

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, newRole);

        var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == newRole);
        var permissions = new List<string>();
        if (roleEntity != null)
        {
            permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleEntity.Id)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }

        var token = GenerateJwtToken(user, newRole, permissions);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserId = user.Id,
            Role = newRole,
            Permissions = permissions
        };
    }

    private string GenerateJwtToken(User user, string role, List<string> permissions)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Role, role)
        };

        foreach (var perm in permissions)
        {
            claims.Add(new Claim("Permission", perm));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
