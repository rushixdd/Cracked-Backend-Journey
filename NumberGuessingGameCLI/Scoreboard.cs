namespace NumberGuessingGameCLI
{
    class Scoreboard
    {
        private int bestScore;

        public Scoreboard()
        {
            bestScore = HighScoreManager.LoadHighScore();
        }

        public Scoreboard(int initialBestScore)
        {
            bestScore = initialBestScore;
        }

        public virtual void UpdateHighScore(int attempts)
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
