using Cards.Data.Game;
using Cards.Data.Game.TrickGame.Cards;

namespace Cards.Services
{
    public class TrickFactory
    {
        public ITrick Build(IEnumerable<Card> cards, Game game) =>
            game switch
            {
                Game.Tarot  => new TarotTrick(cards),
                Game.Belote => throw new NotImplementedException(),
                Game.Poker => throw new ArgumentException("I'm a teapot. (Poker isn't a trick-based game)",
                    nameof(game)),
                _ => throw new ArgumentException("This is not a valid game", nameof(game))
            };
    }
}
