namespace TaskManagement.Api.DTOs;

public class MediaDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatorId { get; set; } = string.Empty;
}

public class CreateMediaDto
{
    public IFormFile File { get; set; } = null!;
}
