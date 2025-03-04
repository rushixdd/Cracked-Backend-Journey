using ExpenseTrackerCLI.Data;
using ExpenseTrackerCLI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerCLI.Services
{
    public class ExpenseService
    {
        private readonly List<Expense> expenses;
        private readonly IExpenseStorage expenseStorage;
        private readonly IBudgetService budgetService;

        public ExpenseService(IExpenseStorage expenseStorage, IBudgetService budgetService)
        {
            this.expenseStorage = expenseStorage;
            this.budgetService = budgetService;
            expenses = this.expenseStorage.LoadExpenses();
        }


        public void AddExpense(string description, decimal amount, string category = "Uncategorized")
        {
            int id = expenses.Count > 0 ? expenses.Max(e => e.Id) + 1 : 1;

            Expense newExpense = new()
            {
                Id = id,
                Description = description,
                Amount = amount,
                Date = DateTime.Now,
                Category = category
            };

            expenses.Add(newExpense);
            this.expenseStorage.SaveExpenses(expenses);
            Console.WriteLine($"✅ Expense added successfully (ID: {id}, Category: {category})");
        }

        public void ListExpenses(string sortBy = "default")
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("⚠ No expenses found.");
                return;
            }

            List<Expense> sortedExpenses = [.. expenses];
            sortedExpenses = sortBy.ToLower() switch
            {
                "date" => [.. sortedExpenses.OrderByDescending(e => e.Date)],
                "amount" => [.. sortedExpenses.OrderByDescending(e => e.Amount)],
                _ => [.. sortedExpenses.OrderBy(e => e.Date)],
            };
            Console.WriteLine("\n=====================================================");
            Console.WriteLine("  ID    Date          Description      Amount  ");
            Console.WriteLine("=====================================================");

            foreach (var expense in sortedExpenses)
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Date:yyyy-MM-dd}   {expense.Description,-15} ₹{expense.Amount,8:F2}");
            }

            Console.WriteLine("=====================================================\n");
        }

        public void UpdateExpense(int id, decimal? newAmount = null, string? newDescription = null, string? newCategory = null)
        {
            if (expenses.Count == 0 || expenses.All(e => e.Id != id))
            {
                Console.WriteLine($"❌ Expense with ID {id} not found.");
                return;
            }

            var expense = expenses.First(e => e.Id == id);

            if (newAmount.HasValue)
                expense.Amount = newAmount.Value;

            if (!string.IsNullOrEmpty(newDescription))
                expense.Description = newDescription;

            if (!string.IsNullOrEmpty(newCategory))
                expense.Category = newCategory;

            this.expenseStorage.SaveExpenses(expenses);
            Console.WriteLine($"✅ Expense updated successfully (ID: {id}, Category: {expense.Category})");
        }



        public void DeleteExpense(int id)
        {
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                expenses.Remove(expense);
                this.expenseStorage.SaveExpenses(expenses);
                Console.WriteLine($"✅ Expense deleted successfully (ID: {id})");
            }
            else
            {
                Console.WriteLine($"❌ Expense with ID {id} not found.");
            }
        }

        public void ShowSummary()
        {
            decimal totalExpense = expenses.Sum(e => e.Amount);
            Console.WriteLine("\n=========================");
            Console.WriteLine("   Expense Summary");
            Console.WriteLine("=========================");
            Console.WriteLine($"Total Expenses: ₹{totalExpense}");
            ApplyBudgetWarnings(totalExpense, new BudgetService().GetBudget());

            Console.WriteLine("=========================\n");
        }

        public void ShowMonthlySummary(int month)
        {
            var monthlyExpenses = expenses
        .Where(e => e.Date.Month == month && e.Date.Year == DateTime.Now.Year)
        .ToList();

            decimal totalMonthlyExpense = monthlyExpenses.Sum(e => e.Amount);

            Console.WriteLine($"\n=========================");
            Console.WriteLine($"  Summary for {new DateTime(DateTime.Now.Year, month, 1):MMMM}");
            Console.WriteLine("=========================");
            Console.WriteLine($"Total Expenses: ₹{totalMonthlyExpense}");
            ApplyBudgetWarnings(totalMonthlyExpense, new BudgetService().GetBudget());
            Console.WriteLine("=========================\n");
        }

        private void ApplyBudgetWarnings(decimal totalExpense, decimal budget)
        {
            if (budget > 0)
            {
                decimal percentageUsed = (totalExpense / budget) * 100;

                if (totalExpense > budget)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"🔴 WARNING: You have exceeded your budget of ₹{budget}!");
                }
                else if (percentageUsed >= 90)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"🟡 ALERT: You have used {percentageUsed:F1}% of your budget (₹{budget}).");
                }
                Console.ResetColor();
            }
        }

        public Expense? GetExpenseById(int id)
        {
            return expenses.FirstOrDefault(e => e.Id == id);
        }
    }
}
