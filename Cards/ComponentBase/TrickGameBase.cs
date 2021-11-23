using Cards.Data.Game.TrickGame.Cards;
using Cards.Services;

using Microsoft.AspNetCore.Components;

namespace Cards.ComponentBase
{
    public abstract class TrickGameBase : CardGameBase
    {
        protected TrickGameBase(HttpContextService httpContextService, NavigationManager navigationManager)
            : base(httpContextService, navigationManager)
        {
        }

        protected CardStack Stack { get; init; } = null!;
    }
}
