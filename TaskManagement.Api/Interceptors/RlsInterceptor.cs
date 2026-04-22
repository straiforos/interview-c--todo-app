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
            using var command = connection.CreateCommand();
            var userId = _currentUserService.UserId;
            var role = _currentUserService.Role ?? "User"; // Default to User if not found
            
            if (!string.IsNullOrEmpty(userId))
            {
                command.CommandText = $"SET ROLE app_user; SET SESSION app.current_user_id = '{userId}'; SET SESSION app.user_role = '{role}';";
            }
            else
            {
                // Reset role and session for unauthenticated requests (e.g., login/register)
                // to prevent connection pooling from leaking state.
                command.CommandText = "RESET ROLE; RESET app.current_user_id; RESET app.user_role;";
            }
            
            await command.ExecuteNonQueryAsync(cancellationToken);
            await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }

        public override void ConnectionOpened(
            DbConnection connection,
            ConnectionEndEventData eventData)
        {
            using var command = connection.CreateCommand();
            var userId = _currentUserService.UserId;
            var role = _currentUserService.Role ?? "User";
            
            if (!string.IsNullOrEmpty(userId))
            {
                command.CommandText = $"SET ROLE app_user; SET SESSION app.current_user_id = '{userId}'; SET SESSION app.user_role = '{role}';";
            }
            else
            {
                command.CommandText = "RESET ROLE; RESET app.current_user_id; RESET app.user_role;";
            }
            
            command.ExecuteNonQuery();
            base.ConnectionOpened(connection, eventData);
        }
}
