using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GitHubActivityCLI
{
    class ActivityParser
    {
        public static List<string> ParseAndDisplayActivity(string json, string? eventType = null)
        {
            var formattedEvents = new List<string>();
            try
            {
                var events = JsonNode.Parse(json)?.AsArray();

                if (events == null || events.Count == 0)
                {
                    Console.WriteLine("\u001b[33m⚠ No recent activity found.\u001b[0m"); // Yellow text
                    return formattedEvents;
                }

                Console.WriteLine("\u001b[34m══════════════════════════════════════════════════════\u001b[0m");
                Console.WriteLine("\u001b[36m📌 GitHub Activity:\u001b[0m");
                Console.WriteLine("\u001b[34m══════════════════════════════════════════════════════\u001b[0m");

                foreach (var ev in events)
                {
                    string type = ev?["type"]?.ToString();
                    string repo = ev?["repo"]?["name"]?.ToString();
                    string description = GetEventDescription(type);

                    if (!string.IsNullOrEmpty(eventType) && type != eventType)
                    {
                        continue; // Skip events that don't match the filter
                    }

                    string eventColor = GetEventColor(type);
                    Console.WriteLine($"{eventColor}▶ {description}\u001b[0m → \u001b[32m{repo}\u001b[0m");
                    formattedEvents.Add($"▶ {description} → {repo}");
                }

                Console.WriteLine("\u001b[34m══════════════════════════════════════════════════════\u001b[0m");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error parsing activity: {ex.Message}");
            }

            return formattedEvents;
        }

        private static string GetEventDescription(string eventType)
        {
            return eventType switch
            {
                "PushEvent" => "Pushed commits",
                "PullRequestEvent" => "Created or updated a pull request",
                "IssuesEvent" => "Opened or commented on an issue",
                "ForkEvent" => "Forked a repository",
                "WatchEvent" => "Starred a repository",
                "CreateEvent" => "Created a branch or repository",
                "DeleteEvent" => "Deleted a branch or repository",
                "ReleaseEvent" => "Published a new release",
                _ => "Performed an action"
            };
        }

        private static string GetEventColor(string eventType)
        {
            return eventType switch
            {
                "PushEvent" => "\u001b[32m", // Green
                "PullRequestEvent" => "\u001b[35m", // Purple
                "IssuesEvent" => "\u001b[33m", // Yellow
                "ForkEvent" => "\u001b[36m", // Cyan
                "WatchEvent" => "\u001b[34m", // Blue
                _ => "\u001b[37m" // White
            };
        }
    }
}
