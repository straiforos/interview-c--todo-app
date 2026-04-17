namespace TaskManagement.Api.Models;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    public virtual User User { get; set; } = null!;

    public int? TaskId { get; set; }
    public virtual TaskItem? Task { get; set; }
}
