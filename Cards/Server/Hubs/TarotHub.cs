using Cards.Data.Game;
using Cards.Server.Clients;

namespace Cards.Server.Hubs
{
    public class TarotHub : TrickGameHub<ITarotClient>
    {
        private IList<Card> Dog { get; set; } = new List<Card>();

        private static byte MaxDogCount => Players.Count switch
        {
            3 or 4 => 6,
            5 => 3,
            _ => throw new InvalidOperationException("TarotHub.MaxDogCount is only accessible with 3, 4 or 5 players.")
        };

        protected override IList<Card> Deck { get; init; } = GenerateDeck();

        private static IList<Card> GenerateDeck()
        {
            IList<Card> deck = new List<Card>();
            Random random = new();
            byte[] cardIndexes = Enumerable
                .Repeat((byte)0, 78)
                .Select(i => (byte)random.Next(0, 78))
                .ToArray();
            for (Card.CardColor c = Card.CardColor.Diamond; c <= Card.CardColor.Trump; c++)
                if (c < Card.CardColor.Trump)
                    for (Card.CardHeight h = Card.CardHeight.Ace; h <= Card.CardHeight.King; h++)
                        deck.Add(new(Card.CardDeck.Tarot, c, h));
                else
                    for (Card.CardHeight h = Card.CardHeight.Magician; h <= Card.CardHeight.Fool; h++)
                        deck.Add(new(Card.CardDeck.Tarot, c, h));
            return deck.OrderBy((card) => cardIndexes[deck.IndexOf(card)]).ToList();
        }

        protected override IList<IList<Card>> DealHands()
        {
            static IEnumerable<IList<Card>> makeHands()
            {
                for (byte i = 0; i < Players.Count; i++)
                    yield return new List<Card>();
            }
            IList<IList<Card>> hands = new List<IList<Card>>(makeHands());
            Random random = new();
            byte i = 1;
            byte n = 1;
            while (i <= this.Deck.Count)
            {
                hands[n % Players.Count] = hands[n++ % Players.Count]
                        .Concat(new List<Card>() { this.Deck[i++], this.Deck[i++], this.Deck[i++] })
                        .ToList();
                Console.WriteLine(i);
                if ((random.NextBoolean() && this.Dog.Count < MaxDogCount)
                    || this.Deck.Count - i < (MaxDogCount - this.Dog.Count) * 4)
                    this.Dog.Add(this.Deck[i++]);
                Console.WriteLine(i);
            }
            return hands;
        }
    }
}
