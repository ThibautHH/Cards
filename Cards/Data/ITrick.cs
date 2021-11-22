namespace Cards.Data
{
    public interface ITrick
    {
        IList<Card> Cards { get; }

        double ComputeScore();
    }
}