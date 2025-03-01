using System;

namespace TaskTrackerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: task-cli <command> [arguments]");
                return;
            }

            TaskManager taskManager = new TaskManager();
            string command = args[0].ToLower();

            switch (command)
            {
                case "add":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: task-cli add \"Task Description\"");
                    }
                    else
                    {
                        taskManager.AddTask(args[1]);
                    }
                    break;

                case "list":
                    string status = args.Length > 1 ? args[1] : "";
                    taskManager.ListTasks(status);
                    break;

                case "update":
                    if (args.Length < 3 || !int.TryParse(args[1], out int updateId))
                    {
                        Console.WriteLine("Usage: task-cli update <id> \"New Description\"");
                    }
                    else
                    {
                        taskManager.UpdateTask(updateId, args[2]);
                    }
                    break;

                case "delete":
                    if (args.Length < 2 || !int.TryParse(args[1], out int deleteId))
                    {
                        Console.WriteLine("Usage: task-cli delete <id>");
                    }
                    else
                    {
                        taskManager.DeleteTask(deleteId);
                    }
                    break;

                case "mark-in-progress":
                    if (args.Length < 2 || !int.TryParse(args[1], out int progressId))
                    {
                        Console.WriteLine("Usage: task-cli mark-in-progress <id>");
                    }
                    else
                    {
                        taskManager.ChangeStatus(progressId, "in-progress");
                    }
                    break;

                case "mark-done":
                    if (args.Length < 2 || !int.TryParse(args[1], out int doneId))
                    {
                        Console.WriteLine("Usage: task-cli mark-done <id>");
                    }
                    else
                    {
                        taskManager.ChangeStatus(doneId, "done");
                    }
                    break;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
