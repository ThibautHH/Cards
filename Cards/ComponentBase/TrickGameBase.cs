using Cards.Pages.Game.TrickGame;
using Cards.Services;

using Microsoft.AspNetCore.Components;

namespace Cards.ComponentBase
{
    public abstract class TrickGameBase : CardGameBase
    {
        protected TrickFactory _trickFactory;

        protected TrickGameBase(HttpContextService httpContextService, NavigationManager navigationManager, TrickFactory trickFactory)
            : base(httpContextService, navigationManager)
        {
            this._trickFactory = trickFactory;
        }

        protected CardStack Stack { get; init; } = null!;
    }
}
