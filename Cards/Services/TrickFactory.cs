using Cards.Data;

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
                    Data.Game.Tarot => new TarotTrick(cards),
                    Data.Game.Belote => new BeloteTrick(cards)
                };
    }
}