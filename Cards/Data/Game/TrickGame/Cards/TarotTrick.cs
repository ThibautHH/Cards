namespace Cards.Data.Game.TrickGame.Cards
{
    public class TarotTrick : ITrick
    {
        public TarotTrick(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
                if (card.Deck != Card.CardDeck.Tarot)
                    throw new ArgumentException("Cannot use cards not in the Tarot deck.", nameof(cards));
            this.Cards = cards.ToList();
        }

        public IList<Card> Cards { get; }

        public Card WinningCard =>
            this.Cards
                .OrderByDescending(card => card.Height)
                .First(card =>
                    card.Height < Card.CardHeight.Fool
                );

        public double ComputeScore()
        {
            double score = 0.0;
            foreach (Card card in this.Cards)
                score += card.Height switch
                {
                    Card.CardHeight.Jack => 1.5,
                    Card.CardHeight.Knight => 2.5,
                    Card.CardHeight.Queen => 3.5,
                    Card.CardHeight.King
                    or Card.CardHeight.Magician
                    or Card.CardHeight.World
                    or Card.CardHeight.Fool => 4.5,
                    _ => 0.5,
                };
            return score;
        }
    }
}