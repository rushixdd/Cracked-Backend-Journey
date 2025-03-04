using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuessingGameCLI
{
    class Game
    {
        private NumberGenerator numberGenerator;
        private Difficulty difficulty;
        private HintSystem hintSystem;
        private Scoreboard scoreboard;
        private GameTimer gameTimer;

        public Game()
        {
            numberGenerator = new NumberGenerator();
            difficulty = new Difficulty();
            hintSystem = new HintSystem();
            scoreboard = new Scoreboard();
            gameTimer = new GameTimer();
        }

        public void Start()
        {
            UI.ShowWelcomeMessage();
            bool playAgain = true;

            while (playAgain)
            {
                Play();
                playAgain = UI.AskToPlayAgain();
            }
        }

        private void Play()
        {
            int maxAttempts = difficulty.SelectDifficulty();
            int secretNumber = numberGenerator.Generate();
            int attempts = 0;
            gameTimer.Start();

            while (attempts < maxAttempts)
            {
                int userGuess = UI.GetUserGuess();
                attempts++;

                if (userGuess == secretNumber)
                {
                    gameTimer.Stop();
                    UI.ShowWinMessage(attempts, gameTimer.ElapsedTime());
                    scoreboard.UpdateHighScore(attempts);
                    return;
                }
                else
                {
                    UI.ShowHint(userGuess, secretNumber);
                    hintSystem.ProvideHint(userGuess, secretNumber);
                }
            }

            UI.ShowLoseMessage(secretNumber);
        }
    }
}
