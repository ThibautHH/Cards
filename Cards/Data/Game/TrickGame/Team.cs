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

        public Team(IEnumerable<Player> players)
        {
            this.Players = players.ToList();
            this.Tricks ??= null!;
        }

        public IList<Player> Players { get; init; }

        public Side TeamSide { get; init; }

        public IList<ITrick> Tricks { get; init; }

        public abstract int GetScore();
    }
}