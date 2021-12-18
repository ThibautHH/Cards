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

            this.Color = color switch
            {
                < CardColor.Diamond or > CardColor.Trump =>
                    throw new ArgumentOutOfRangeException(nameof(color), (int)color, "This is not a valid color."),
                CardColor.Trump => this.Deck != CardDeck.Tarot ? 
                    throw new ArgumentException("Cannot set card color to trump" +
                        " if it is not a Tarot card.", nameof(color))
                    : color,
                _ => color
            };

            this.Height = height is < CardHeight.Ace or > CardHeight.Fool
                ? throw new ArgumentOutOfRangeException(nameof(height), (int)height, "This is not a valid height.")
                : this.Deck != CardDeck.Tarot
                && height is >= CardHeight.Magician or CardHeight.Knight
                ? throw new ArgumentOutOfRangeException(nameof(height), (int)height, "Cannot set card height to trump or Knight " +
                    "if it is not a Tarot card.")
                : this.Deck == CardDeck.Player32
                && height is <= CardHeight.Six and not CardHeight.Ace
                ? throw new ArgumentOutOfRangeException(nameof(height), (int)height, "Cannot set card height to lower than seven " +
                    "if it is a 32-cards deck card.")
                : this.Color == CardColor.Trump
                && height is < CardHeight.Magician
                ? throw new ArgumentOutOfRangeException(nameof(height), (int)height, "Cannot set card height to lower than Magician " +
                    "if it is a trump card.")
                : this.Color != CardColor.Trump
                && height is > CardHeight.King
                ? throw new ArgumentOutOfRangeException(nameof(height), (int)height, "Cannot set card height to higher than King " +
                    "if it is not a trump card.")
                : height;

            this.Player = player!;
        }

        public CardDeck Deck { get; init; }

        public CardColor Color { get; init; }

        public CardHeight Height { get; init; }

        public Player Player { get; init; }

        public void Play(Player player, ref CardStack stack)
        {
            if (player.Game is Game.Tarot or Game.Belote)
            {
                Card playedCard = new(this.Deck, this.Color, this.Height, player);
                stack.Stack.Cards.Add(playedCard);
            } else
                throw new ArgumentOutOfRangeException(nameof(player.Game), player.Game, "Cannot play a card if the game isn't trick-based.");
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