using NUnit.Framework;
using NumberGuessingGameCLI;
using System;
using System.IO;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class DifficultyTests
    {
        private StringReader stringReader;
        private StringWriter stringWriter;
        private TextReader originalInput;
        private TextWriter originalOutput;

        [SetUp]
        public void SetUp()
        {
            originalInput = Console.In;
            originalOutput = Console.Out;
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetIn(originalInput);
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }

        [Test]
        public void SelectDifficulty_Easy_Returns10()
        {
            stringReader = new StringReader("1");
            Console.SetIn(stringReader);

            Difficulty difficulty = new Difficulty();
            int result = difficulty.SelectDifficulty();

            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public void SelectDifficulty_Medium_Returns5()
        {
            stringReader = new StringReader("2");
            Console.SetIn(stringReader);

            Difficulty difficulty = new Difficulty();
            int result = difficulty.SelectDifficulty();

            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void SelectDifficulty_Hard_Returns3()
        {
            stringReader = new StringReader("3");
            Console.SetIn(stringReader);

            Difficulty difficulty = new Difficulty();
            int result = difficulty.SelectDifficulty();

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void SelectDifficulty_Invalid_Returns5()
        {
            stringReader = new StringReader("invalid");
            Console.SetIn(stringReader);

            Difficulty difficulty = new Difficulty();
            int result = difficulty.SelectDifficulty();

            Assert.That(result, Is.EqualTo(5));
        }
    }
}
