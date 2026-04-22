using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.DTOs;

/// <summary>
/// Summary DTO for listing tasks.
/// </summary>
public class TaskSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

/// <summary>
/// Detailed DTO for viewing or editing a task.
/// </summary>
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatorUsername { get; set; } = string.Empty;
}

public class CreateTaskDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class UpdateTaskDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}
