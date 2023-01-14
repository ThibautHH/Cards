namespace Cards.ComponentBase
{
    public abstract class TrickGameBase : CardGameBase
    {
        protected CardStackBase Stack { get; init; } = null!;
    }
}
