namespace ExpenseTrackerCLI.Services
{
    public class BudgetService
    {
        private const string BudgetFile = "budget.json";

        public decimal GetBudget()
        {
            if (!File.Exists(BudgetFile))
                return 0;

            string json = File.ReadAllText(BudgetFile);
            return decimal.Parse(json);
        }

        public void SetBudget(decimal amount)
        {
            File.WriteAllText(BudgetFile, amount.ToString());
            Console.WriteLine($"✅ Monthly budget set to ₹{amount:F2}");
        }
    }
}
