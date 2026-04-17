using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Interceptors;

/// <summary>
/// Interceptor for enforcing PostgreSQL Row Level Security (RLS).
/// Injects the current user's ID into the database session.
/// </summary>
public class RlsInterceptor : DbConnectionInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public RlsInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override async Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        if (!string.IsNullOrEmpty(userId))
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SET LOCAL app.current_user_id = '{userId}';";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    public override void ConnectionOpened(
        DbConnection connection,
        ConnectionEndEventData eventData)
    {
        var userId = _currentUserService.UserId;
        if (!string.IsNullOrEmpty(userId))
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SET LOCAL app.current_user_id = '{userId}';";
            command.ExecuteNonQuery();
        }

        base.ConnectionOpened(connection, eventData);
    }
}
