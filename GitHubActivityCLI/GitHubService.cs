namespace GitHubActivityCLI
{
    class GitHubService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task FetchGitHubActivity(string username)
        {
            string url = $"https://api.github.com/users/{username}/events";

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0"); // Required for GitHub API

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error: Unable to fetch data. Status Code: {response.StatusCode}");
                    return;
                }

                string json = await response.Content.ReadAsStringAsync();
                ActivityParser.ParseAndDisplayActivity(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
