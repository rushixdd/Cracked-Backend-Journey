using NUnit.Framework;
using NumberGuessingGameCLI;

namespace NumberGuessingGameCLI.Tests
{
    [TestFixture]
    public class NumberGeneratorTests
    {
        [Test]
        public void Generate_ReturnsNumberWithinRange()
        {
            NumberGenerator numberGenerator = new NumberGenerator();
            for (int i = 0; i < 1000; i++)
            {
                int result = numberGenerator.Generate();
                Assert.That(result, Is.InRange(1, 100));
            }
        }
    }
}
