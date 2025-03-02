using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GitHubActivityCLI
{
    class ActivityParser
    {
        public static void ParseAndDisplayActivity(string json)
        {
            JsonArray events = JsonNode.Parse(json)?.AsArray();

            if (events == null || events.Count == 0)
            {
                Console.WriteLine("⚠️ No recent activity found.");
                return;
            }

            Console.WriteLine("\n📌 Recent GitHub Activity:");

            foreach (var e in events)
            {
                string type = e["type"]?.ToString();
                string repo = e["repo"]?["name"]?.ToString();

                if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(repo))
                {
                    Console.WriteLine($"- {FormatEventType(type)} in {repo}");
                }
            }
        }

        private static string FormatEventType(string type)
        {
            return type switch
            {
                "PushEvent" => "Pushed commits",
                "IssuesEvent" => "Opened an issue",
                "WatchEvent" => "Starred a repo",
                "ForkEvent" => "Forked a repo",
                _ => "Performed an activity"
            };
        }
    }
}
