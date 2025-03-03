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
        private List<Expense> expenses;

        public ExpenseService()
        {
            expenses = ExpenseStorage.LoadExpenses();
        }

        public void AddExpense(string description, decimal amount, string category = "Uncategorized")
        {
            int id = expenses.Count > 0 ? expenses.Max(e => e.Id) + 1 : 1;

            Expense newExpense = new Expense
            {
                Id = id,
                Description = description,
                Amount = amount,
                Date = DateTime.Now,
                Category = category
            };

            expenses.Add(newExpense);
            ExpenseStorage.SaveExpenses(expenses);
            Console.WriteLine($"✅ Expense added successfully (ID: {id}, Category: {category})");
        }

        public void ListExpenses()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("⚠ No expenses found.");
                return;
            }

            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine($"{"ID",-5} {"Date",-12} {"Description",-20} {"Amount",-8}");
            Console.WriteLine("═══════════════════════════════════════════════════");

            foreach (var expense in expenses)
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Date.ToString("d"),-12} {expense.Description,-20} ₹{expense.Amount,-8:F2}");
            }

            Console.WriteLine("═══════════════════════════════════════════════════");
        }

        public void DeleteExpense(int id)
        {
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                expenses.Remove(expense);
                ExpenseStorage.SaveExpenses(expenses);
                Console.WriteLine($"✅ Expense deleted successfully (ID: {id})");
            }
            else
            {
                Console.WriteLine($"❌ Expense with ID {id} not found.");
            }
        }

        public void ShowSummary()
        {
            decimal total = expenses.Sum(e => e.Amount);
            Console.WriteLine($"💰 Total expenses: ₹{total}");
        }

        public void ShowMonthlySummary(int month)
        {
            var expenses = ExpenseStorage.LoadExpenses().Where(e => e.Date.Month == month && e.Date.Year == DateTime.Now.Year).ToList();
            decimal total = expenses.Sum(e => e.Amount);
            decimal budget = new BudgetService().GetBudget();

            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine($"📅 Summary for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {DateTime.Now.Year}");
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine($"💰 Total expenses: ₹{total:F2}");

            if (budget > 0)
            {
                Console.WriteLine($"🎯 Budget: ₹{budget:F2}");
                if (total > budget)
                {
                    Console.WriteLine($"🚨 Warning: You exceeded your budget by ₹{(total - budget):F2}!");
                }
            }
            Console.WriteLine("═══════════════════════════════════════════════════");
        }
    }
}
