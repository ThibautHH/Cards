using Cards.Data.Game;
using Cards.Data.Game.TrickGame;
using Cards.Data.Game.TrickGame.Cards;

namespace Cards.ComponentBase
{
    public abstract class CardStackBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        public abstract ITrick Stack { get; set; }

        public void AssignTrick(IEnumerable<Team> teams) =>
            teams.SingleOrDefault(team =>
                    team.Players.SingleOrDefault(player => this.Stack.WinningCard.Player == player) is not null)
              ?.Tricks.Add(this.Stack);
    }
}
