namespace Cards
{
    public class Random : System.Random
    {
        private uint _boolBits;

        public Random() : base() { }
        public Random(int seed) : base(seed) { }

        public bool NextBoolean()
        {
            _boolBits >>= 1;
            if (_boolBits <= 1)
                _boolBits = (uint)~this.Next();
            return (_boolBits & 1) == 0;
        }
    }
}
