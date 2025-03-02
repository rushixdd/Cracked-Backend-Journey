namespace GitHubActivityCLI
{
    class GitHubService
    {
        private readonly HttpClient _client;

        public GitHubService(HttpClient? httpClient = null)
        {
            _client = httpClient ?? new HttpClient(); 
        }

        public async Task FetchGitHubActivity(string username, string? eventType = null)
        {
            string url = string.Format(Constants.GitHubApiUrl, username);

            _client.DefaultRequestHeaders.UserAgent.ParseAdd(Constants.UserAgent); // Required for GitHub API

            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);

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
