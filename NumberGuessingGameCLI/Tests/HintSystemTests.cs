using NUnit.Framework;
using NumberGuessingGameCLI;
using System;
using System.IO;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class HintSystemTests
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        [SetUp]
        public void SetUp()
        {
            originalOutput = Console.Out;
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }

        [Test]
        public void ProvideHint_VeryClose_PrintsVeryClose()
        {
            HintSystem hintSystem = new HintSystem();
            hintSystem.ProvideHint(5, 7);

            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("🔥 Very close!"));
        }

        [Test]
        public void ProvideHint_Close_PrintsClose()
        {
            HintSystem hintSystem = new HintSystem();
            hintSystem.ProvideHint(5, 14);

            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("⚠️ Close!"));
        }

        [Test]
        public void ProvideHint_FarAway_PrintsFarAway()
        {
            HintSystem hintSystem = new HintSystem();
            hintSystem.ProvideHint(5, 20);

            string output = stringWriter.ToString().Trim();
            Assert.That(output, Is.EqualTo("❄️ Far away!"));
        }
    }
}
