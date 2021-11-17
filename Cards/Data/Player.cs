namespace Cards.Data
{
    public class Player
    {
        public Player(string name, Game game)
        {
            this.Name = name;
            this.Game = game;
        }

        public string Name { get; init; }

        public Game Game { get; init; }

        public bool Ready { get; private set; }

        public void SetReady() => this.Ready = true;

        public void Unready() => this.Ready = false;

        public static bool operator ==(Player left, Player right) =>
            left.Name == right.Name
            && left.Game == right.Game;

        public static bool operator !=(Player left, Player right) =>
            left.Name != right.Name
            && left.Game != right.Game;

        public override bool Equals(object? obj) =>
            obj is Player player
            && player == this;

        public override int GetHashCode() =>
            System.HashCode.Combine(this.Name, this.Game);
    }
}
