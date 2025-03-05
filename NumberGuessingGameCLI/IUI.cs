namespace NumberGuessingGameCLI
{
    public interface IUI
    {
        void ShowWelcomeMessage();
        int GetUserGuess();
        void ShowHint(int guess, int secretNumber);
        void ShowWinMessage(int attempts, string timeTaken);
        void ShowLoseMessage(int secretNumber);
        bool AskToPlayAgain();
    }
}
