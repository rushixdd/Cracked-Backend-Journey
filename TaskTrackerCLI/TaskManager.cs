using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TaskTrackerCLI
{
    public class TaskManager
    {
        private const string FilePath = "tasks.json";
        private List<TaskModel> tasks = new List<TaskModel>();

        public TaskManager()
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                tasks = JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new List<TaskModel>();
            }
        }

        private void SaveTasks()
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public void AddTask(string description)
        {
            int newId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
            tasks.Add(new TaskModel { Id = newId, Description = description });
            SaveTasks();
            Console.WriteLine($"Task added successfully (ID: {newId})");
        }

        public void ListTasks(string status = "")
        {
            var filteredTasks = string.IsNullOrEmpty(status) ? tasks : tasks.FindAll(t => t.Status == status);
            if (filteredTasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            foreach (var task in filteredTasks)
            {
                Console.WriteLine($"[{task.Id}] {task.Description} - {task.Status} (Created: {task.CreatedAt})");
            }
        }

        public void UpdateTask(int id, string newDescription)
        {
            var task = tasks.Find(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            task.Description = newDescription;
            task.UpdatedAt = DateTime.Now;
            SaveTasks();
            Console.WriteLine($"Task {id} updated.");
        }

        public void DeleteTask(int id)
        {
            var task = tasks.Find(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            tasks.Remove(task);
            SaveTasks();
            Console.WriteLine($"Task {id} deleted.");
        }

        public void ChangeStatus(int id, string status)
        {
            var task = tasks.Find(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            task.Status = status;
            task.UpdatedAt = DateTime.Now;
            SaveTasks();
            Console.WriteLine($"Task {id} marked as {status}.");
        }

        public void SearchTasks(string keyword)
        {
            var results = tasks.Where(t => t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine($"\n🔍 Search Results for '{keyword}':");
            foreach (var task in results)
                Console.WriteLine($"[{task.Id}] {task.Description} ({task.Status})");
        }

        public void FilterTasksByDate(string fromDate, string toDate)
        {
            if (!DateTime.TryParse(fromDate, out DateTime from) || !DateTime.TryParse(toDate, out DateTime to))
            {
                Console.WriteLine("❌ Invalid date format. Use YYYY-MM-DD.");
                return;
            }

            var results = tasks.Where(t => t.CreatedAt >= from && t.CreatedAt <= to);
            Console.WriteLine($"\n📆 Tasks from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}:");
            foreach (var task in results)
                Console.WriteLine($"[{task.Id}] {task.Description} ({task.Status}) - Created: {task.CreatedAt}");
        }
    }
}
