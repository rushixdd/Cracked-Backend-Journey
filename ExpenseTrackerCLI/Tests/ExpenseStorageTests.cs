using NUnit.Framework;
using ExpenseTrackerCLI.Data;
using ExpenseTrackerCLI.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ExpenseTrackerCLI.Tests
{
    [TestFixture]
    public class ExpenseStorageTests
    {
        private ExpenseStorage _expenseStorage;
        private string _testFilePath = "test_expenses.json";

        [SetUp]
        public void SetUp()
        {
            _expenseStorage = new ExpenseStorage { FilePath = _testFilePath };
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
        public void LoadExpenses_FileDoesNotExist_ReturnsEmptyList()
        {
            var result = _expenseStorage.LoadExpenses();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void LoadExpenses_FileExists_ReturnsExpensesList()
        {
            var expenses = new List<Expense>
            {
                new Expense { Id = 1, Description = "Test Expense 1", Amount = 100, Date = DateTime.Now, Category = "Test" },
                new Expense { Id = 2, Description = "Test Expense 2", Amount = 200, Date = DateTime.Now, Category = "Test" }
            };
            var json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_testFilePath, json);
            var result = _expenseStorage.LoadExpenses();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Description, Is.EqualTo("Test Expense 1"));
            Assert.That(result[1].Description, Is.EqualTo("Test Expense 2"));
        }

        [Test]
        public void SaveExpenses_WritesExpensesToFile()
        {
            var expenses = new List<Expense>
            {
                new Expense { Id = 1, Description = "Test Expense 1", Amount = 100, Date = DateTime.Now, Category = "Test" },
                new Expense { Id = 2, Description = "Test Expense 2", Amount = 200, Date = DateTime.Now, Category = "Test" }
            };

            _expenseStorage.SaveExpenses(expenses);
            Assert.That(File.Exists(_testFilePath), Is.True);
            var json = File.ReadAllText(_testFilePath);
            var result = JsonSerializer.Deserialize<List<Expense>>(json);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Description, Is.EqualTo("Test Expense 1"));
            Assert.That(result[1].Description, Is.EqualTo("Test Expense 2"));
        }
    }
}
