using Cards.Data.Game;
using Cards.Server.Clients;

using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.Hubs
{
    public class PlayerListHub : Hub<IPlayerListClient>
    {
        public enum Action
        {
            Signup,
            Quit,
            Ready,
            Unready
        }

        public async Task EnterLobby(string game) =>
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, Group(game));

        public async Task Update(Action action, string playerName, string game) =>
            await this.Clients.Group(Group(game)).Update(action, MakePlayer(playerName, game));

        private static string Group(string game) => $"{game}Players";

        private static Player MakePlayer(string name, string game) =>
            new(name, Enum.Parse<Game>(game));
    }
}
