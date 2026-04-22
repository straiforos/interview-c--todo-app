namespace TaskManagement.Api.Models;

/// <summary>
/// Represents a task in the system.
/// </summary>
public class TaskItem : IBaseEntity, ISoftDeletable
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public string CreatorId { get; set; } = string.Empty;
    public virtual User Creator { get; set; } = null!;
}
