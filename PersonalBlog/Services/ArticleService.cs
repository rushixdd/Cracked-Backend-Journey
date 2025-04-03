using System.Text.Json;
using PersonalBlog.Models;
using PersonalBlog.Pages;

namespace PersonalBlog.Services
{
    public class ArticleService
    {
        private readonly string _articlesDirectory = "Data/Articles";

        public ArticleService()
        {
            if (!Directory.Exists(_articlesDirectory))
            {
                Directory.CreateDirectory(_articlesDirectory);
            }
        }

        public List<Article> GetAllArticles()
        {
            var files = Directory.GetFiles(_articlesDirectory, "*.json");
            var articles = new List<Article>();

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var article = JsonSerializer.Deserialize<Article>(json);
                if (article != null)
                {
                    articles.Add(article);
                }
            }

            return articles.OrderByDescending(a => a.PublishedDate).ToList();
        }

        public Article? GetArticleById(string id)
        {
            var filePath = Path.Combine(_articlesDirectory, $"{id}.json");
            if (!File.Exists(filePath)) return null;

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Article>(json);
        }

        public void AddArticle(Article article)
        {
            var filePath = Path.Combine(_articlesDirectory, $"{article.Id}.json");
            var json = JsonSerializer.Serialize(article, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void UpdateArticle(Article updatedArticle)
        {
            var filePath = Path.Combine(_articlesDirectory, $"{updatedArticle.Id}.json");
            if (!File.Exists(filePath)) return;

            var json = JsonSerializer.Serialize(updatedArticle, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void DeleteArticle(string id)
        {
            var filePath = Path.Combine(_articlesDirectory, $"{id}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
