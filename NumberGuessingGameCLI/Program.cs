using NumberGuessingGameCLI;
class Program
{
    static void Main()
    {
        NumberGenerator numberGenerator = new NumberGenerator();
        Difficulty difficulty = new Difficulty();
        HintSystem hintSystem = new HintSystem();
        Scoreboard scoreboard = new Scoreboard();
        GameTimer gameTimer = new GameTimer();
        IUI ui = new UI();

        Game game = new Game(numberGenerator, difficulty, hintSystem, scoreboard, gameTimer, ui);
        game.Start();
    }
}
