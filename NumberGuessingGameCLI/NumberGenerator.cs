namespace NumberGuessingGameCLI
{
    class NumberGenerator
    {
        private Random random = new Random();
        public int Generate()
        {
            return random.Next(1, 101);
        }
    }
}
