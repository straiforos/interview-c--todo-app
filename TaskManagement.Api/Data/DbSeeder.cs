using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        // 1. Seed Permissions
        var permissions = Enum.GetValues<AppPermission>();
        var existingPermissions = await context.Permissions.ToDictionaryAsync(p => p.Name);

        foreach (var permission in permissions)
        {
            var name = permission.ToName();
            if (!existingPermissions.ContainsKey(name))
            {
                context.Permissions.Add(new Permission { Name = name });
            }
        }
        await context.SaveChangesAsync();

        // 2. Seed Default Role
        var defaultRoleName = "User";
        if (!await roleManager.RoleExistsAsync(defaultRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(defaultRoleName));
        }

        // 3. Assign Permissions to Default Role
        var role = await roleManager.FindByNameAsync(defaultRoleName);
        var rolePermissions = await context.RolePermissions
            .Where(rp => rp.RoleId == role!.Id)
            .Select(rp => rp.PermissionId)
            .ToListAsync();

        var allPermissions = await context.Permissions.ToListAsync();

        foreach (var permission in allPermissions)
        {
            if (!rolePermissions.Contains(permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = role!.Id,
                    PermissionId = permission.Id
                });
            }
        }
        await context.SaveChangesAsync();
    }
}
