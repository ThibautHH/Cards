using Cards.Server.Clients;

using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.Hubs
{
    public class PlayerListHub : Hub<IPlayerListClient>
    {
        public async Task Play(string playerName, string group)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, group);
            await this.Clients.Group(group).Update(playerName);
        }
    }
}
