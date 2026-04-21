using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using TaskManagement.Api.Data;

namespace TaskManagement.Api.Interceptors;

/// <summary>
/// Handles authorization for <see cref="PermissionRequirement"/>.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionAuthorizationHandler"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
    /// <param name="cache">The memory cache for performance.</param>
    public PermissionAuthorizationHandler(IServiceProvider serviceProvider, IMemoryCache cache)
    {
        _serviceProvider = serviceProvider;
        _cache = cache;
    }

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return;

        // Cache permissions per user for 1 minute to balance performance and real-time updates
        var cacheKey = $"UserPermissions_{userId}";
        if (!_cache.TryGetValue(cacheKey, out HashSet<string>? permissions))
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var userRoles = await dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            permissions = (await dbContext.RolePermissions
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Name)
                .ToListAsync())
                .ToHashSet();

            _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(1));
        }

        if (permissions != null && permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
