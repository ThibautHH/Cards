using Cards.Data.Game;
using Cards.Server.Hubs;

namespace Cards.Server.Clients
{
    public interface IPlayerListClient
    {
        Task Update(PlayerListHub.Action action, Player player);
    }
}