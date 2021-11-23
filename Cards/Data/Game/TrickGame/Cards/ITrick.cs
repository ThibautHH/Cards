namespace Cards.Data.Game.TrickGame.Cards
{
    public interface ITrick
    {
        IList<Card> Cards { get; }

        double ComputeScore();
    }
}