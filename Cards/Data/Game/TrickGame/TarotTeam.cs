using System.Diagnostics.Metrics;

using Cards.Data.Game.TrickGame.Cards;

using Microsoft.AspNetCore.Routing.Constraints;

namespace Cards.Data.Game.TrickGame
{
    public class TarotTeam : Team
    {
        public enum Contract
        {
            Take,
            Guard,
            GuardWithout,
            GuardAgainst
        }

        public TarotTeam(
            IEnumerable<Player> players,
            Side side,
            IEnumerable<TarotTrick> tricks,
            Contract? contract = null)
            : base(players)
        {
            if (contract is null)
            {
                this.TeamSide = side is Side.Defense
                    ? side
                    : throw new ArgumentException("Must specify a contract if the Team is attacking.",
                        nameof(contract));
                this.TeamContract = null;
            } else
            {
                this.TeamContract = side is Side.Attack
                    ? contract
                    : throw new ArgumentException("Cannot specify a contract if the Team is defending.",
                        nameof(contract));
                this.TeamSide = side;
            }
            this.Tricks = (IList<ITrick>)tricks.ToList();
        }

        private Contract? TeamContract { get; init; }

        public int OudlersCount =>
            this.Tricks.SelectMany(trick => trick.Cards)
               .Count(card => card.Height is Card.CardHeight.Magician
                                             or Card.CardHeight.World
                                             or Card.CardHeight.Fool);

        public override int GetScore()
        {
            double score = this.Tricks.Sum(trick => trick.ComputeScore());
            return this.TeamSide is Side.Attack
                ? (int)Math.Floor(score)
                : (int)Math.Ceiling(score);
        }
    }
}
