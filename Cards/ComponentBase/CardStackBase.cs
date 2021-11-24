using Cards.Data.Game;
using Cards.Data.Game.TrickGame;
using Cards.Data.Game.TrickGame.Cards;

namespace Cards.ComponentBase
{
    public abstract class CardStackBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        public CardStackBase(ITrick trick)
        {
            this.Stack = trick;
        }

        public ITrick Stack { get; }

        public void AssignTrick(IEnumerable<Team> teams)
        {
            foreach (Team team in teams)
            {
                if (team is null)
                    throw new ArgumentException("Passed IEnumerable<Team> is empty or contains null.", nameof(teams));
                foreach (Player player in team.Players)
                    if (this.Stack.WinningCard.Player == player)
                        team.Tricks.Add(this.Stack);
            }
        }
    }
}