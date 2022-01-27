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

        public async Task Play()
        {
            Console.WriteLine(this.Context.ConnectionId);
            Players.Add(this.Context.ConnectionId);
            Console.WriteLine(Players.Count);
        }

        public async Task Quit()
        {
            Console.WriteLine(this.Context.ConnectionId);
            Players.Remove(this.Context.ConnectionId);
            Console.WriteLine(Players.Count);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(this.Context.ConnectionId);
            Players.Remove(this.Context.ConnectionId);
            Console.WriteLine(Players.Count);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
