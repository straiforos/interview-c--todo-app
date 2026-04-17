using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.Api.Services;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);
        await base.OnConnectedAsync();
    }
}
