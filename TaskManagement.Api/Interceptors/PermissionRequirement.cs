using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Api.Interceptors;

/// <summary>
/// Authorization requirement for a specific permission.
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the name of the required permission.
    /// </summary>
    public string Permission { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
    /// </summary>
    /// <param name="permission">The permission name.</param>
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

/// <summary>
/// Attribute to enforce permission-based authorization on controllers or actions.
/// </summary>
public class HasPermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HasPermissionAttribute"/> class using a strongly-typed permission.
    /// </summary>
    /// <param name="permission">The permission enum value.</param>
    public HasPermissionAttribute(AppPermission permission) : base(policy: permission.ToName())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HasPermissionAttribute"/> class using a string policy.
    /// </summary>
    /// <param name="policy">The policy name.</param>
    public HasPermissionAttribute(string policy) : base(policy: policy)
    {
    }
}
