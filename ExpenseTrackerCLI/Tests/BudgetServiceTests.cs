using NUnit.Framework;
using ExpenseTrackerCLI.Data;
using System.IO;

namespace ExpenseTrackerCLI.Tests
{
    [TestFixture]
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;
        private string _testFilePath = "budget.json";

        [SetUp]
        public void SetUp()
        {
            _budgetService = new BudgetService();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Test]
        public void GetBudget_FileDoesNotExist_ReturnsZero()
        {
            var result = _budgetService.GetBudget();
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void GetBudget_FileExists_ReturnsBudget()
        {
            var budget = 5000m;
            File.WriteAllText(_testFilePath, budget.ToString());
            var result = _budgetService.GetBudget();
            Assert.That(result, Is.EqualTo(budget));
        }

        [Test]
        public void SetBudget_WritesBudgetToFile()
        {
            var budget = 7500m;
            _budgetService.SetBudget(budget);
            Assert.That(File.Exists(_testFilePath), Is.True);
            var result = decimal.Parse(File.ReadAllText(_testFilePath));
            Assert.That(result, Is.EqualTo(budget));
        }
    }
}
