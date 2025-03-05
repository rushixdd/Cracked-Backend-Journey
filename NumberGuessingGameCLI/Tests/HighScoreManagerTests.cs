using NUnit.Framework;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class HighScoreManagerTests
    {
        private const string TestFilePath = "test_highscores.json";

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
        public void LoadHighScore_FileDoesNotExist_ReturnsMaxValue()
        {
            int result = HighScoreManager.LoadHighScore();
            Assert.That(result, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void LoadHighScore_FileExists_ReturnsCorrectValue()
        {
            File.WriteAllText(TestFilePath, "5");
            int result = HighScoreManager.LoadHighScore();
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void SaveHighScore_SavesCorrectValue()
        {
            HighScoreManager.SaveHighScore(3);
            string content = File.ReadAllText(TestFilePath);
            Assert.That(content, Is.EqualTo("3"));
        }
    }
}
