using ExpenseTrackerCLI.Services;

class Program
{
    static void Main(string[] args)
    {
        var service = new ExpenseService();

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
                service.ListExpenses();
                break;

            case "delete":
                if (args.Length < 2) { Console.WriteLine("❌ Usage: delete <id>"); return; }
                if (int.TryParse(args[1], out int id))
                    service.DeleteExpense(id);
                else
                    Console.WriteLine("❌ Invalid ID.");
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
