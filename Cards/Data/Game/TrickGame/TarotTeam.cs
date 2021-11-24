using Cards.Data.Game.TrickGame.Cards;

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
            Team.Side side,
            IEnumerable<TarotTrick> tricks,
            Contract? contract = null)
            : base(players)
        {
            if (contract is null)
            {
                this.TeamSide = side is Side.Defense
                    ? side
                    : throw new ArgumentException(
                        "Must specify a contract if the Team is attacking.",
                        nameof(contract)
                        );
                this.TeamContract = null;
            } else
            {
                this.TeamContract = side is Side.Attack
                    ? contract
                    : throw new ArgumentException(
                        "Cannot specify a contract if the Team is defending.",
                        nameof(contract)
                        );
                this.TeamSide = side;
            }

            this.Tricks = (IList<ITrick>)tricks.ToList();
        }

        public Contract? TeamContract { get; init; }

        public int OudlersCount
        {
            get
            {
                int count = 0;
                foreach (ITrick trick in this.Tricks)
                    foreach (Card card in trick.Cards)
                        if (card.Height is Card.CardHeight.Magician
                            or Card.CardHeight.World
                            or Card.CardHeight.Fool)
                            count++;
                return count;
            }
        }

        public override int GetScore()
        {
            double score = 0.0;
            foreach (TarotTrick trick in this.Tricks)
                score += trick.ComputeScore();
            return score % 1 != 0 && this.TeamSide == Side.Defense
                ? (int)score + 1
                : (int)score;
        }
    }
}