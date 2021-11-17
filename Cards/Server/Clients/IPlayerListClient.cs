using Cards.Data;

namespace Cards.Server.Clients
{
    public interface IPlayerListClient
    {
        Task Signup(Player player);

        Task Quit(Player player);

        Task Ready(Player player);

        Task Unready(Player player);
    }
}