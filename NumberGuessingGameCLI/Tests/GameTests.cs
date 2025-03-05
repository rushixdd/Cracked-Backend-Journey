using NUnit.Framework;
using Moq;
using NumberGuessingGameCLI;
using System;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class GameTests
    {
        private Mock<NumberGenerator> mockNumberGenerator;
        private Mock<Difficulty> mockDifficulty;
        private Mock<HintSystem> mockHintSystem;
        private Mock<Scoreboard> mockScoreboard;
        private Mock<GameTimer> mockGameTimer;
        private Mock<IUI> mockUI;
        private Game game;

        [SetUp]
        public void SetUp()
        {
            mockNumberGenerator = new Mock<NumberGenerator>();
            mockDifficulty = new Mock<Difficulty>();
            mockHintSystem = new Mock<HintSystem>();
            mockScoreboard = new Mock<Scoreboard>();
            mockGameTimer = new Mock<GameTimer>();
            mockUI = new Mock<IUI>();

            game = new Game(
                mockNumberGenerator.Object,
                mockDifficulty.Object,
                mockHintSystem.Object,
                mockScoreboard.Object,
                mockGameTimer.Object,
                mockUI.Object
            );
        }

        [Test]
        public void Start_ShowsWelcomeMessage()
        {
            mockUI.Setup(ui => ui.AskToPlayAgain()).Returns(false);

            game.Start();

            mockUI.Verify(ui => ui.ShowWelcomeMessage(), Times.Once);
        }

        [Test]
        public void Play_GuessesCorrectNumber_ShowsWinMessage()
        {
            mockDifficulty.Setup(d => d.SelectDifficulty()).Returns(5);
            mockNumberGenerator.Setup(ng => ng.Generate()).Returns(42);
            mockUI.SetupSequence(ui => ui.GetUserGuess())
                .Returns(50)
                .Returns(42);

            game.Start();

            mockUI.Verify(ui => ui.ShowWinMessage(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockScoreboard.Verify(sb => sb.UpdateHighScore(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Play_RunsOutOfAttempts_ShowsLoseMessage()
        {
            mockDifficulty.Setup(d => d.SelectDifficulty()).Returns(2);
            mockNumberGenerator.Setup(ng => ng.Generate()).Returns(42);
            mockUI.SetupSequence(ui => ui.GetUserGuess())
                .Returns(50)
                .Returns(30);

            game.Start();

            mockUI.Verify(ui => ui.ShowLoseMessage(42), Times.Once);
        }

        [Test]
        public void Play_ProvidesHint()
        {
            mockDifficulty.Setup(d => d.SelectDifficulty()).Returns(2);
            mockNumberGenerator.Setup(ng => ng.Generate()).Returns(42);
            mockUI.SetupSequence(ui => ui.GetUserGuess())
                .Returns(50)
                .Returns(30);

            game.Start();

            mockUI.Verify(ui => ui.ShowHint(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            mockHintSystem.Verify(hs => hs.ProvideHint(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        }
    }
}
