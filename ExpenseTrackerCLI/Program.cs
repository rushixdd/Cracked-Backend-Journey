using ExpenseTrackerCLI.Data;
using ExpenseTrackerCLI.Services;

class Program
{
    static void Main(string[] args)
    {
        var service = new ExpenseService(new ExpenseStorage(), new BudgetService());

        if (args.Length == 0)
        {
            Console.WriteLine("🚀 Expense Tracker CLI\nUsage:");
            Console.WriteLine("  add <description> <amount>  - Add an expense");
            Console.WriteLine("  list                       - Show all expenses");
            Console.WriteLine("  delete <id>                - Delete an expense");
            Console.WriteLine("  summary                     - Show total expenses");
            return;
        }

        string command = args[0].ToLower();
        switch (command)
        {
            case "add":
                if (args.Length < 3)
                {
                    Console.WriteLine("❌ Usage: expense-tracker add <description> <amount> [category]");
                    return;
                }

                string description = args[1];
                decimal amount;
                if (!decimal.TryParse(args[2], out amount) || amount <= 0)
                {
                    Console.WriteLine("❌ Invalid amount. Please enter a positive number.");
                    return;
                }

                string category = args.Length > 3 ? args[3] : "Uncategorized";
                service.AddExpense(description, amount, category);
                break;

            case "list":
                string sortBy = args.Length > 1 ? args[1] : "default";
                service.ListExpenses(sortBy);
                break;

            case "update":
                if (args.Length < 3)
                {
                    Console.WriteLine("❌ Usage: expense-tracker update --id <id> [--amount <newAmount>] [--description <newDescription>] [--category <newCategory>]");
                    return;
                }

                int updateId = 0;
                decimal? newAmount = null;
                string? newDescription = null;
                string? newCategory = null;

                for (int i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--id":
                            if (i + 1 < args.Length && int.TryParse(args[i + 1], out updateId))
                                i++;
                            else
                            {
                                Console.WriteLine("❌ Invalid or missing ID.");
                                return;
                            }
                            break;

                        case "--amount":
                            if (i + 1 < args.Length && decimal.TryParse(args[i + 1], out decimal amount1) && amount1 > 0)
                            {
                                newAmount = amount1;
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("❌ Invalid amount. Please enter a positive number.");
                                return;
                            }
                            break;

                        case "--description":
                            if (i + 1 < args.Length)
                            {
                                newDescription = args[i + 1];
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("❌ Missing description.");
                                return;
                            }
                            break;

                        case "--category":
                            if (i + 1 < args.Length)
                            {
                                newCategory = args[i + 1];
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("❌ Missing category.");
                                return;
                            }
                            break;
                    }
                }

                if (updateId == 0)
                {
                    Console.WriteLine("❌ Please provide a valid expense ID.");
                    return;
                }

                service.UpdateExpense(updateId, newAmount, newDescription, newCategory);
                break;

            case "delete":
                if (args.Length < 2)
                {
                    Console.WriteLine("❌ Usage: expense-tracker delete --id <id>");
                    return;
                }

                if (!int.TryParse(args[1], out int deleteId))
                {
                    Console.WriteLine("❌ Invalid ID. Please enter a valid numeric ID.");
                    return;
                }

                var expenseToDelete = service.GetExpenseById(deleteId);
                if (expenseToDelete == null)
                {
                    Console.WriteLine($"❌ Expense with ID {deleteId} not found.");
                    return;
                }

                Console.Write($"⚠ Are you sure you want to delete this expense? (ID: {deleteId}, {expenseToDelete.Description}, ₹{expenseToDelete.Amount}) [y/N]: ");
                string? confirmation = Console.ReadLine()?.Trim().ToLower();

                if (confirmation == "y")
                {
                    service.DeleteExpense(deleteId);
                    Console.WriteLine($"✅ Expense (ID: {deleteId}) deleted successfully.");
                }
                else
                {
                    Console.WriteLine("❌ Deletion canceled.");
                }
                break;

            case "summary":
                if (args.Length == 1)
                {
                    service.ShowSummary();
                }
                else if (args.Length == 3 && args[1] == "--month" && int.TryParse(args[2], out int month) && month >= 1 && month <= 12)
                {
                    service.ShowMonthlySummary(month);
                }
                else
                {
                    Console.WriteLine("❌ Usage: expense-tracker summary [--month <1-12>]");
                }
                break;
            case "set-budget":
                if (args.Length < 2 || !decimal.TryParse(args[1], out decimal budgetAmount))
                {
                    Console.WriteLine("❌ Usage: expense-tracker set-budget <amount>");
                    return;
                }
                new BudgetService().SetBudget(budgetAmount);
                break;


            default:
                Console.WriteLine("❌ Unknown command.");
                break;
        }
    }
}
