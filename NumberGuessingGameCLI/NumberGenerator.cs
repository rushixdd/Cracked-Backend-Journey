namespace NumberGuessingGameCLI
{
    public class NumberGenerator
    {
        private Random random = new Random();
        public virtual int Generate()
        {
            return random.Next(1, 101);
        }
    }
}
