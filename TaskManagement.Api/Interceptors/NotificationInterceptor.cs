using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Interceptors;

/// <summary>
/// Interceptor for automatically creating notifications when tasks are assigned.
/// Implements an aspect-oriented approach to side-effects.
/// </summary>
public class NotificationInterceptor : SaveChangesInterceptor
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly List<Notification> _pendingNotifications = new();

    public NotificationInterceptor(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not AppDbContext context) return result;

        var entries = context.ChangeTracker.Entries<TaskItem>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var task = entry.Entity;
            string? newAssigneeId = null;
            bool shouldNotify = false;

            if (entry.State == EntityState.Added)
            {
                if (!string.IsNullOrEmpty(task.AssigneeId) && task.AssigneeId != task.CreatorId)
                {
                    newAssigneeId = task.AssigneeId;
                    shouldNotify = true;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                var originalAssigneeId = entry.Property(t => t.AssigneeId).OriginalValue;
                if (!string.IsNullOrEmpty(task.AssigneeId) && 
                    task.AssigneeId != originalAssigneeId && 
                    task.AssigneeId != task.CreatorId)
                {
                    newAssigneeId = task.AssigneeId;
                    shouldNotify = true;
                }
            }

            if (shouldNotify && newAssigneeId != null)
            {
                var notification = new Notification
                {
                    UserId = newAssigneeId,
                    Message = $"You have been assigned a task: {task.Title}",
                    TaskId = task.Id // Note: For Added state, ID might be 0 until Saved
                };
                
                context.Notifications.Add(notification);
                _pendingNotifications.Add(notification);
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        foreach (var notification in _pendingNotifications)
        {
            await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Message,
                notification.TaskId,
                notification.CreatedAt
            }, cancellationToken);
        }

        _pendingNotifications.Clear();
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
