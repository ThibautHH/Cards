using Cards.Data.Game;
using Cards.Server.Clients;

using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.Hubs
{
    public abstract class CardGameHub<TClient> : Hub<TClient> where TClient : class, ICardGameClient
    {
        protected IList<Card> _deck = null!;

        protected abstract IList<Card> Deck { get; init; }

        protected static IList<string> Players { get; private set; } = new List<string>();

        protected abstract IList<IList<Card>> DealHands();

        public async Task Launch()
        {
            IList<IList<Card>> hands = this.DealHands();
            for (int i = 0; i < Players.Count; i++)
                await this.Clients.Client(Players[i]).Start(hands[i]);
        }

        public Task Play()
        {
            Players.Add(this.Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task Quit()
        {
            Players.Remove(this.Context.ConnectionId);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Players.Remove(this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
