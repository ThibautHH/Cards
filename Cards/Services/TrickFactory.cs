using Cards.Data.Game;
using Cards.Data.Game.TrickGame.Cards;

namespace Cards.Services
{
    public class TrickFactory
    {
        public TrickFactory() 
        {
        }
        
        public ITrick Build(IEnumerable<Card> cards, Game game) =>
            game switch
                {
                    Game.Tarot => new TarotTrick(cards),
                    Game.Belote => new BeloteTrick(cards)
                };
    }
}