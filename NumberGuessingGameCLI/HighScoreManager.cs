using System.Text.Json;

namespace NumberGuessingGameCLI
{
    class HighScoreManager
    {
        private static string FilePath = "highscores.json";

        public static void SetFilePath(string filePath)
        {
            FilePath = filePath;
        }

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
