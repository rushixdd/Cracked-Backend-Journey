using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Data
{
    public static class ExpenseStorage
    {
        private static readonly string FilePath = "expenses.json";

        public static List<Expense> LoadExpenses()
        {
            if (!File.Exists(FilePath))
                return new List<Expense>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
        }

        public static void SaveExpenses(List<Expense> expenses)
        {
            var json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
