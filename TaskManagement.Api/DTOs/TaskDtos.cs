using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RichTextContent { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatorId { get; set; } = string.Empty;
    public string? AssigneeId { get; set; }
}

public class CreateTaskDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string? RichTextContent { get; set; }

    public DateTime? DueDate { get; set; }

    public string? AssigneeId { get; set; }
}

public class UpdateTaskDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string? RichTextContent { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public string? AssigneeId { get; set; }
}
