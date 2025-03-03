﻿namespace ExpenseTrackerCLI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = String.Empty;
    }
}
