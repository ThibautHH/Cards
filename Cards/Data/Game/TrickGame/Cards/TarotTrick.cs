namespace Cards.Data.Game.TrickGame.Cards
{
    public class TarotTrick : ITrick
    {
        public TarotTrick(IEnumerable<Card> cards)
        {
            this.Cards = cards.ToList();
        }

        public IList<Card> Cards { get; }

        public Card WinningCard =>
            this.Cards
               .OrderByDescending(card => card.Height)
               .First(card =>
                    card.Height < Card.CardHeight.Fool);

        public double ComputeScore() =>
            this.Cards.Sum(card => card.Height switch
            {
                Card.CardHeight.Jack   => 1.5,
                Card.CardHeight.Knight => 2.5,
                Card.CardHeight.Queen  => 3.5,
                Card.CardHeight.King
                    or Card.CardHeight.Magician
                    or Card.CardHeight.World
                    or Card.CardHeight.Fool => 4.5,
                _ => 0.5
            });
    }
}
