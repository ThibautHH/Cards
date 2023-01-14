using Cards.Data.Game.TrickGame.Cards;

namespace Cards.Data.Game.TrickGame
{
    public abstract class Team
    {
        public enum Side : byte
        {
            Attack,
            Defense
        }

        protected Team(IEnumerable<Player> players)
        {
            this.Players = players.ToList();
            this.Tricks ??= null!;
        }

        public IList<Player> Players { get; init; }

        protected Side TeamSide { get; init; }

        public IList<ITrick> Tricks { get; init; }

        public abstract int GetScore();
    }
}
