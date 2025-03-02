namespace GitHubActivityCLI
{
    class GitHubService
    {
        private static readonly HttpClient client = new();

        public static async Task FetchGitHubActivity(string username, string? eventType = null)
        {
            string url = string.Format(Constants.GitHubApiUrl, username);

            client.DefaultRequestHeaders.UserAgent.ParseAdd(Constants.UserAgent); // Required for GitHub API

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error: Unable to fetch data. Status Code: {response.StatusCode}");
                    return;
                }

                string? json = await response.Content.ReadAsStringAsync();
                ActivityParser.ParseAndDisplayActivity(json, eventType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
