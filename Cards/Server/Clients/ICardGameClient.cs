using Cards.Data.Game;

namespace Cards.Server.Clients
{
    public interface ICardGameClient
    {
        Task Start(IEnumerable<Card> hand);
    }
}