using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Api.Models;

/// <summary>
/// Represents a granular permission in the system.
/// </summary>
public class Permission
{
    /// <summary>
    /// Gets or sets the unique identifier for the permission.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission (e.g., "Tasks.Delete").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the roles that have this permission.
    /// </summary>
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

/// <summary>
/// Join entity for the many-to-many relationship between roles and permissions.
/// </summary>
public class RolePermission
{
    /// <summary>
    /// Gets or sets the role identifier.
    /// </summary>
    public string RoleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the permission identifier.
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Gets or sets the role associated with this permission.
    /// </summary>
    public virtual IdentityRole Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets the permission associated with this role.
    /// </summary>
    public virtual Permission Permission { get; set; } = null!;
}
