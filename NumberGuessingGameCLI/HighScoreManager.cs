using System.Text.Json;

namespace NumberGuessingGameCLI
{
    class HighScoreManager
    {
        private const string FilePath = "highscores.json";

        public static int LoadHighScore()
        {
            if (!File.Exists(FilePath)) return int.MaxValue;

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<int>(json);
        }

        public static void SaveHighScore(int attempts)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(attempts));
        }

    }
}
