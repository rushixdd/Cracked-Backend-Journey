namespace ExpenseTrackerCLI.Data
{
    public interface IBudgetService
    {
        decimal GetBudget();
        void SetBudget(decimal amount);
    }
}