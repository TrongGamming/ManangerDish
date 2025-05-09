using Microsoft.AspNetCore.SignalR;

namespace ManagerDish.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendRefresh(string groupName)
        {
            await Clients.Group(groupName).SendAsync("Refresh", "Refresh success");
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}
