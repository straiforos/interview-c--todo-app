using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Api.Models;

public class User : IdentityUser
{
    public virtual ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
    public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
