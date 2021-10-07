namespace Cards.Data
{
    public readonly struct Card
    {
        public Card(Deck deck, CardColor color, CardHeight height)
        {
            this.Deck = deck;

            this.Color = this.Deck != Deck.Tarot
            ? color switch
            {
                CardColor.Trump =>
                    throw new ArgumentException("Cannot set card color to trump" +
                    " if it is not a Tarot card.", nameof(color)),
                _ => color
            } : color;

            this.Height = this.Deck != Deck.Tarot
            ? height switch
            {
                (>= CardHeight.Magician) or CardHeight.Knight =>
                    throw new ArgumentException("Cannot set card height to trump or Knight" +
                    " if it is not a Tarot card.", nameof(height)),
                _ => height
            }
            : this.Deck == Deck.Player32
            ? height switch
            {
                (<= CardHeight.Six) and not CardHeight.Ace =>
                    throw new ArgumentException("Cannot set card height to lower than seven" +
                    " if it is a 32-cards deck card.", nameof(height)),
                _ => height
            } : height;
        }

        public Deck Deck { get; init; }

        public CardColor Color { get; init; }

        public CardHeight Height { get; init; }
    }

    public enum Deck : byte
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