using Cards.Data.Game.TrickGame;
using Cards.Pages.Game.TrickGame;
using Cards.Services;

using Microsoft.AspNetCore.Components;

namespace Cards.ComponentBase
{
    public abstract class TrickGameBase<TTeam> : CardGameBase where TTeam : Team
    {
        [Inject]
        protected TrickFactory TrickFactory { get; set; } = null!;

        protected CardStack Stack { get; init; } = null!;

        protected abstract IList<TTeam> Teams { get; init; }
    }
}
