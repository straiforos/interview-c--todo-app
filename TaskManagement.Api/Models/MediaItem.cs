namespace TaskManagement.Api.Models;

public class MediaItem : IBaseEntity, ISoftDeletable
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string CreatorId { get; set; } = string.Empty;
    public virtual User Creator { get; set; } = null!;
}
