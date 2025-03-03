using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Data
{
    public class ExpenseStorage
    {
        private string filePath = "expenses.json";
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public List<Expense> LoadExpenses()
        {
            if (!File.Exists(filePath))
                return [];

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? [];
        }

        public void SaveExpenses(List<Expense> expenses)
        {
            var json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
