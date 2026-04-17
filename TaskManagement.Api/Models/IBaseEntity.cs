namespace TaskManagement.Api.Models;

/// <summary>
/// Base interface for all domain entities.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// The unique identifier for the entity.
    /// </summary>
    int Id { get; set; }
}

/// <summary>
/// Interface for entities that support soft deletion.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was deleted.
    /// </summary>
    DateTime? DeletedAt { get; set; }
}
