using System;
namespace GitHubActivityCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("❌ Error: Please provide a GitHub username.\nUsage: github-activity <username> [eventType]");
                return;
            }

            string username = args[0];
            string? eventType = args.Length > 1 ? args[1] : null;

            HttpClient httpClient = new HttpClient();
            GitHubService gitHubService = new GitHubService(httpClient);

            await gitHubService.FetchGitHubActivity(username, eventType);
        }
    }
}