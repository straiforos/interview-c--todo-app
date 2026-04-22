namespace TaskManagement.Api.Models;

/// <summary>
/// Defines the canonical list of functional permissions in the system.
/// </summary>
public enum AppPermission
{
    // Task Permissions
    [PermissionName("Tasks.Read")]
    TasksRead = 1,
    
    [PermissionName("Tasks.Create")]
    TasksCreate = 2,
    
    [PermissionName("Tasks.Update")]
    TasksUpdate = 3,
    
    [PermissionName("Tasks.Delete")]
    TasksDelete = 4,
}

/// <summary>
/// Attribute to associate a string constant with a permission enum member.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class PermissionNameAttribute : Attribute
{
    public string Name { get; }
    public PermissionNameAttribute(string name) => Name = name;
}

/// <summary>
/// Extension methods for AppPermission.
/// </summary>
public static class AppPermissionExtensions
{
    /// <summary>
    /// Retrieves the string name associated with the permission.
    /// </summary>
    public static string ToName(this AppPermission permission)
    {
        var field = permission.GetType().GetField(permission.ToString());
        var attribute = field?.GetCustomAttributes(typeof(PermissionNameAttribute), false)
            .Cast<PermissionNameAttribute>()
            .FirstOrDefault();
            
        return attribute?.Name ?? permission.ToString();
    }
}
