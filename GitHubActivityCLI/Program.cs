using System;
namespace GitHubActivityCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("❌ Error: Please provide a GitHub username.\nUsage: github-activity <username>");
                return;
            }

            string username = args[0];
            await GitHubService.FetchGitHubActivity(username);
        }
    }
}