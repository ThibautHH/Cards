using Cards.Data;
using Cards.Server.Clients;

using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.Hubs
{
    public enum PlayerListAction
    {
        Signup,
        Quit,
        Ready,
        Unready
    }

    public class PlayerListHub : Hub<IPlayerListClient>
    {
        public async Task EnterLobby(string gameGroup) =>
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameGroup);

        public async Task Update(PlayerListAction action, string playerName, string game)
        {
            switch (action)
            {
                case PlayerListAction.Signup:
                    await this.Clients.Group(Group(game))
                        .Signup(
                            MakePlayer(playerName, game)
                        );
                    break;
                case PlayerListAction.Quit:
                    await this.Clients.Group(Group(game))
                        .Quit(
                            MakePlayer(playerName, game)
                        );
                    break;
                case PlayerListAction.Ready:
                    await this.Clients.Group(Group(game))
                        .Ready(
                            MakePlayer(playerName, game)
                        );
                    break;
                case PlayerListAction.Unready:
                    await this.Clients.Group(Group(game))
                        .Unready(
                            MakePlayer(playerName, game)
                        );
                    break;
            }
        }

        private static string Group(string game) => $"{game}Players";

        private static Player MakePlayer(string name, string game) =>
            new(name, Enum.Parse<Game>(game));
    }
}
