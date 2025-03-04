using NUnit.Framework;
using ExpenseTrackerCLI.Data;
using ExpenseTrackerCLI.Models;
using ExpenseTrackerCLI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExpenseTrackerCLI.Tests
{
    [TestFixture]
    public class ExpenseServiceTests
    {
        private ExpenseService _expenseService;
        private Mock<IExpenseStorage> _mockExpenseStorage;
        private Mock<IBudgetService> _mockBudgetService;
        private List<Expense> _testExpenses;

        [SetUp]
        public void SetUp()
        {
            _testExpenses = new List<Expense>
            {
                new Expense { Id = 1, Description = "Test Expense 1", Amount = 100, Date = DateTime.Now, Category = "Test" },
                new Expense { Id = 2, Description = "Test Expense 2", Amount = 200, Date = DateTime.Now, Category = "Test" }
            };

            _mockExpenseStorage = new Mock<IExpenseStorage>();
            _mockExpenseStorage.Setup(es => es.LoadExpenses()).Returns(_testExpenses);

            _mockBudgetService = new Mock<IBudgetService>();

            _expenseService = new ExpenseService(_mockExpenseStorage.Object, _mockBudgetService.Object);
        }

        [Test]
        public void AddExpense_AddsExpenseToListAndSaves()
        {
            _expenseService.AddExpense("New Expense", 300, "New Category");
            Assert.That(_testExpenses.Count, Is.EqualTo(3));
            Assert.That(_testExpenses.Last().Description, Is.EqualTo("New Expense"));
            _mockExpenseStorage.Verify(es => es.SaveExpenses(_testExpenses), Times.Once);
        }

        [Test]
        public void ListExpenses_NoExpenses_PrintsWarning()
        {
            _testExpenses.Clear();
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _expenseService.ListExpenses();
                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo("⚠ No expenses found."));
            }
        }

        [Test]
        public void UpdateExpense_UpdatesExpenseAndSaves()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _expenseService.UpdateExpense(1, newAmount: 150, newDescription: "Updated Expense", newCategory: "Updated Category");
                var updatedExpense = _testExpenses.First(e => e.Id == 1);
                Assert.That(updatedExpense.Amount, Is.EqualTo(150));
                Assert.That(updatedExpense.Description, Is.EqualTo("Updated Expense"));
                Assert.That(updatedExpense.Category, Is.EqualTo("Updated Category"));
                _mockExpenseStorage.Verify(es => es.SaveExpenses(_testExpenses), Times.Once);
            }
        }

        [Test]
        public void DeleteExpense_DeletesExpenseAndSaves()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _expenseService.DeleteExpense(1);
                Assert.That(_testExpenses.Count, Is.EqualTo(1));
                Assert.That(_testExpenses.Any(e => e.Id == 1), Is.False);
                _mockExpenseStorage.Verify(es => es.SaveExpenses(_testExpenses), Times.Once);
            }
        }

        [Test]
        public void ShowSummary_PrintsSummary()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _mockBudgetService.Setup(bs => bs.GetBudget()).Returns(1000m);
                _expenseService.ShowSummary();
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Total Expenses: ₹300"));
            }
        }

        [Test]
        public void ShowMonthlySummary_PrintsMonthlySummary()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _mockBudgetService.Setup(bs => bs.GetBudget()).Returns(1000m);
                _expenseService.ShowMonthlySummary(DateTime.Now.Month);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain($"Summary for {DateTime.Now:MMMM}"));
                Assert.That(result, Does.Contain("Total Expenses: ₹300"));
            }
        }

        [Test]
        public void GetExpenseById_ReturnsCorrectExpense()
        {
            var result = _expenseService.GetExpenseById(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetExpenseById_ExpenseNotFound_ReturnsNull()
        {
            var result = _expenseService.GetExpenseById(999);
            Assert.That(result, Is.Null);
        }
    }
}
