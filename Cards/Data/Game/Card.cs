using Cards.Pages.Game.TrickGame;

namespace Cards.Data.Game
{
    public readonly struct Card
    {
        public Card(
            CardDeck deck,
            CardColor color,
            CardHeight height,
            Player? player = null)
        {
            this.Deck = deck;

            this.Color = this.Deck != CardDeck.Tarot
            ? color switch
            {
                CardColor.Trump =>
                    throw new ArgumentException("Cannot set card color to trump" +
                    " if it is not a Tarot card.", nameof(color)),
                _ => color
            } : color;

            this.Height = this.Deck != CardDeck.Tarot
            ? height switch
            {
                (>= CardHeight.Magician) or CardHeight.Knight =>
                    throw new ArgumentOutOfRangeException(nameof(height), (int)height, "Cannot set card height to trump or Knight " +
                    "if it is not a Tarot card."),
                _ => height
            }
            : this.Deck == CardDeck.Player32
            ? height switch
            {
                (<= CardHeight.Six) and not CardHeight.Ace =>
                    throw new ArgumentOutOfRangeException(nameof(height), (int)height, "Cannot set card height to lower than seven " +
                    "if it is a 32-cards deck card."),
                _ => height
            } : height;

            this.Player = player!;
        }

        public CardDeck Deck { get; init; }

        public CardColor Color { get; init; }

        public CardHeight Height { get; init; }

        public Player Player { get; init; }

        public void Play(Player player, ref CardStack stack)
        {
            Card playedCard = new(this.Deck, this.Color, this.Height, player);
            stack.Stack.Cards.Add(playedCard);
        }

        public enum CardDeck : byte
        {
            Tarot, Player, Player32
        }

        public enum CardColor : byte
        {
            Diamond, Club, Heart, Spade,
            Trump
        }

        public enum CardHeight : byte
        {
            Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
            Jack, Knight, Queen, King,
            Magician, HighPriestess, Empress, Emperor, Hierophant, Lovers, Chariot, Strength, Hermit, WheelofFortune,
            Justice, HangedMan, Death, Temperance, Devil, Tower, Star, Moon, Sun, Judgement, World,
            Fool
        }
    }
}