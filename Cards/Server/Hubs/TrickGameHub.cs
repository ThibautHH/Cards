using Cards.Server.Clients;

namespace Cards.Server.Hubs
{
    public abstract class TrickGameHub<TClient> : CardGameHub<TClient> where TClient : class, ITrickGameClient
    {
    }
}
