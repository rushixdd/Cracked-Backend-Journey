namespace NumberGuessingGameCLI
{
    class Scoreboard
    {
        private int bestScore = HighScoreManager.LoadHighScore();

        public void UpdateHighScore(int attempts)
        {
            if (attempts < bestScore)
            {
                bestScore = attempts;
                Console.WriteLine($"🏆 New High Score: {bestScore} attempts!");
                HighScoreManager.SaveHighScore(attempts);
            }
        }

    }
}
