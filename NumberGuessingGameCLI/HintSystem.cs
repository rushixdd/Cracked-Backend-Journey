namespace NumberGuessingGameCLI
{
    class HintSystem
    {
        public void ProvideHint(int guess, int secretNumber)
        {
            if (Math.Abs(secretNumber - guess) <= 5)
                Console.WriteLine("🔥 Very close!");
            else if (Math.Abs(secretNumber - guess) <= 10)
                Console.WriteLine("⚠️ Close!");
            else
                Console.WriteLine("❄️ Far away!");
        }

    }
}
