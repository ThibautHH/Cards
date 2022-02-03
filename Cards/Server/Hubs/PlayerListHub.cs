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

        private static IDictionary<Game, IList<Player>> MakeLobbies()
        {
            Type gameType = typeof(Game);
            int gameCount = gameType.GetEnumNames().Length;
            IDictionary<Game, IList<Player>> lobbies = new Dictionary<Game, IList<Player>>();
            foreach (Game game in gameType.GetEnumValues())
                lobbies[game] = new List<Player>();
            return lobbies;
        }

        private static readonly IDictionary<Game, IList<Player>> _lobbies = MakeLobbies();

        public Task<bool> ArePlayersReady(Game game)
        {
            bool allReady = true;
            foreach (Player player in _lobbies[game])
                allReady = allReady && player.Ready;
            return Task.FromResult(allReady);
        }

        public async Task EnterLobby(Game game)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, Group(game));
            await this.Clients.Caller.Update(_lobbies[game]);
        }

        public async Task Update(Action action, string playerName, Game game)
        {
            Player player = MakePlayer(playerName, game);
            switch (action)
            {
                case Action.Signup:
                    await this.Signup(player);
                    break;
                case Action.Quit:
                    await this.Quit(player);
                    break;
                case Action.Ready:
                    await this.Ready(player);
                    break;
                case Action.Unready:
                    await this.Unready(player);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, "This is not a valid PlayerListHub.Action.");
            };
            foreach (Player p in _lobbies[game])
                Console.WriteLine(p.Name);
            await this.Clients.Group(Group(game)).Update(_lobbies[game]);
        }

        public Task Signup(Player registeringPlayer)
        {
            if (_lobbies[registeringPlayer.Game].SingleOrDefault(player => player == registeringPlayer) is null)
                _lobbies[registeringPlayer.Game].Add(registeringPlayer);
            return Task.CompletedTask;
        }

        public Task Quit(Player player)
        {
            if (_lobbies[player.Game].SingleOrDefault(quitingPlayer => quitingPlayer == player) is Player quitingPlayer)
                _lobbies[player.Game].Remove(quitingPlayer);
            return Task.CompletedTask;
        }

        public Task Ready(Player player)
        {
            _lobbies[player.Game].Single(playerToReady => playerToReady == player).SetReady();
            return Task.CompletedTask;
        }

        public Task Unready(Player player)
        {
            _lobbies[player.Game].Single(playerToUnready => playerToUnready == player).Unready();
            return Task.CompletedTask;
        }

        private static string Group(Game game) => $"{game}Players";

        private static Player MakePlayer(string name, Game game) => new(name, game);
    }
}
