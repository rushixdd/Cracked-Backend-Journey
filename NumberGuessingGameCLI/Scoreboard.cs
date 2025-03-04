namespace NumberGuessingGameCLI
{
    class Scoreboard
    {
        private int bestScore = int.MaxValue;

        public void UpdateHighScore(int attempts)
        {
            if (attempts < bestScore)
            {
                bestScore = attempts;
                Console.WriteLine($"🏆 New High Score: {bestScore} attempts!");
            }
        }

    }
}
