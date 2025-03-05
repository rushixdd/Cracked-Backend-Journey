using NUnit.Framework;
using NumberGuessingGameCLI;
using System.IO;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class ScoreboardTests
    {
        private const string TestFilePath = "test_highscores.json";
        private Scoreboard scoreboard;

        [SetUp]
        public void SetUp()
        {
            HighScoreManager.SetFilePath(TestFilePath);
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void UpdateHighScore_NewHighScore_UpdatesBestScore()
        {
            HighScoreManager.SaveHighScore(10);
            scoreboard = new Scoreboard(10);
            scoreboard.UpdateHighScore(5);
            int result = HighScoreManager.LoadHighScore();
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void UpdateHighScore_NotHighScore_DoesNotUpdateBestScore()
        {
            HighScoreManager.SaveHighScore(5);
            scoreboard = new Scoreboard(5);
            scoreboard.UpdateHighScore(10);
            int result = HighScoreManager.LoadHighScore();
            Assert.That(result, Is.EqualTo(5));
        }
    }
}
