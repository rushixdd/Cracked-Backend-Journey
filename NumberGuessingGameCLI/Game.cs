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
        private IUI ui;

        public Game(NumberGenerator numberGenerator, Difficulty difficulty, HintSystem hintSystem, Scoreboard scoreboard, GameTimer gameTimer, IUI ui)
        {
            this.numberGenerator = numberGenerator;
            this.difficulty = difficulty;
            this.hintSystem = hintSystem;
            this.scoreboard = scoreboard;
            this.gameTimer = gameTimer;
            this.ui = ui;
        }

        public void Start()
        {
            ui.ShowWelcomeMessage();
            bool playAgain = true;

            while (playAgain)
            {
                Play();
                playAgain = ui.AskToPlayAgain();
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
                int userGuess = ui.GetUserGuess();
                attempts++;

                if (userGuess == secretNumber)
                {
                    gameTimer.Stop();
                    ui.ShowWinMessage(attempts, gameTimer.ElapsedTime());
                    scoreboard.UpdateHighScore(attempts);
                    return;
                }
                else
                {
                    ui.ShowHint(userGuess, secretNumber);
                    hintSystem.ProvideHint(userGuess, secretNumber);
                }
            }

            ui.ShowLoseMessage(secretNumber);
        }
    }
}
