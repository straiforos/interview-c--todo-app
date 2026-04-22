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

        // 2. Seed Roles and Permissions
        var rolesWithPermissions = new Dictionary<string, List<string>>
        {
            { "Admin", new List<string> { "Tasks.Read", "Tasks.Create", "Tasks.Update", "Tasks.Delete" } },
            { "User", new List<string> { "Tasks.Read", "Tasks.Create", "Tasks.Update" } },
            { "ReadOnly", new List<string> { "Tasks.Read" } }
        };

        var allPermissions = await context.Permissions.ToListAsync();

        foreach (var roleConfig in rolesWithPermissions)
        {
            var roleName = roleConfig.Key;
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var role = await roleManager.FindByNameAsync(roleName);
            var currentRolePermissions = await context.RolePermissions
                .Where(rp => rp.RoleId == role!.Id)
                .Include(rp => rp.Permission)
                .ToListAsync();

            var currentPermNames = currentRolePermissions.Select(rp => rp.Permission.Name).ToList();

            // Add missing permissions
            foreach (var permName in roleConfig.Value)
            {
                if (!currentPermNames.Contains(permName))
                {
                    var permission = allPermissions.First(p => p.Name == permName);
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = role!.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            // Remove extra permissions
            foreach (var rp in currentRolePermissions)
            {
                if (!roleConfig.Value.Contains(rp.Permission.Name))
                {
                    context.RolePermissions.Remove(rp);
                }
            }
            
            await context.SaveChangesAsync();
        }
        
        await context.SaveChangesAsync();
    }
}
