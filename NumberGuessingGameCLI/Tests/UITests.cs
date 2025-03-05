using NUnit.Framework;
using NumberGuessingGameCLI;
using System;
using System.IO;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class UITests
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;
        private StringReader stringReader;
        private TextReader originalInput;
        private IUI ui;

        [SetUp]
        public void SetUp()
        {
            originalOutput = Console.Out;
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            originalInput = Console.In;
            ui = new UI();
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();

            Console.SetIn(originalInput);
        }

        [Test]
        public void ShowWelcomeMessage_PrintsWelcomeMessage()
        {
            ui.ShowWelcomeMessage();
            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("🎮 Welcome to the Number Guessing Game!\r\nTry to guess the number between 1 and 100."));
        }

        [Test]
        public void GetUserGuess_ValidInput_ReturnsGuess()
        {
            stringReader = new StringReader("50");
            Console.SetIn(stringReader);

            int result = ui.GetUserGuess();
            Assert.That(result, Is.EqualTo(50));
        }

        [Test]
        public void GetUserGuess_InvalidInput_PromptsAgain()
        {
            stringReader = new StringReader("invalid\r\n50");
            Console.SetIn(stringReader);

            int result = ui.GetUserGuess();
            string output = stringWriter.ToString().Trim();
            Assert.That(result, Is.EqualTo(50));
            Assert.That(output, Does.Contain("❌ Invalid input. Enter a number between 1 and 100:"));
        }

        [Test]
        public void ShowHint_Higher_PrintsHigherHint()
        {
            ui.ShowHint(50, 75);
            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("🔼 Try a higher number."));
        }

        [Test]
        public void ShowHint_Lower_PrintsLowerHint()
        {
            ui.ShowHint(75, 50);
            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("🔽 Try a lower number."));
        }

        [Test]
        public void ShowWinMessage_PrintsWinMessage()
        {
            ui.ShowWinMessage(5, "00:01:30");
            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("🎉 Congratulations! You guessed the number in 5 attempts.\r\n⏱️ Time taken: 00:01:30"));
        }

        [Test]
        public void ShowLoseMessage_PrintsLoseMessage()
        {
            ui.ShowLoseMessage(42);
            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("❌ You ran out of attempts! The number was 42."));
        }

        [Test]
        public void AskToPlayAgain_Yes_ReturnsTrue()
        {
            stringReader = new StringReader("yes");
            Console.SetIn(stringReader);

            bool result = ui.AskToPlayAgain();
            Assert.That(result, Is.True);
        }

        [Test]
        public void AskToPlayAgain_No_ReturnsFalse()
        {
            stringReader = new StringReader("no");
            Console.SetIn(stringReader);

            bool result = ui.AskToPlayAgain();
            Assert.That(result, Is.False);
        }
    }
}
