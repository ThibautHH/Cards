using Cards.Data.Game.TrickGame.Cards;
using Cards.Services;

using Microsoft.AspNetCore.Components;

namespace Cards.ComponentBase
{
    public abstract class TrickGameBase : CardGameBase
    {
        protected TrickGameBase() : base()
        {
        }

        protected CardStackBase Stack { get; init; } = null!;
    }
}
