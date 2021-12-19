using Cards.Data.Game;
using Cards.Server.Clients;

using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.Hubs
{
    public class PlayerListHub : Hub<IPlayerListClient>
    {
        public enum Action : byte
        {
            Signup,
            Quit,
            Ready,
            Unready
        }

        public async Task EnterLobby(Game game) =>
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, Group(game));

        public async Task Update(Action action, string playerName, Game game) =>
            await this.Clients.Group(Group(game)).Update(action, MakePlayer(playerName, game));

        private static string Group(Game game) => $"{game}Players";

        private static Player MakePlayer(string name, Game game) => new(name, game);
    }
}
