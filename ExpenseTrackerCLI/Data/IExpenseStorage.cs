using ExpenseTrackerCLI.Data;
using ExpenseTrackerCLI.Models;

public interface IExpenseStorage
{
    string FilePath { get; set; }
    List<Expense> LoadExpenses();
    void SaveExpenses(List<Expense> expenses);
}
