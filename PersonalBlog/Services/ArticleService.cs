using System.Text.Json;
using PersonalBlog.Models;

namespace PersonalBlog.Services
{
    public class ArticleService
    {
        private readonly string _articlesPath = "Data/articles.json";

        public ArticleService()
        {
            if (!File.Exists(_articlesPath))
            {
                Directory.CreateDirectory("Data");
                File.WriteAllText(_articlesPath, "[]");
            }
        }

        public List<Article> GetAllArticles()
        {
            var json = File.ReadAllText(_articlesPath);
            return JsonSerializer.Deserialize<List<Article>>(json) ?? new List<Article>();
        }

        public Article? GetArticleById(string id)
        {
            return GetAllArticles().FirstOrDefault(a => a.Id == id);
        }

        public void AddArticle(Article article)
        {
            var articles = GetAllArticles();
            articles.Add(article);
            SaveArticles(articles);
        }

        public void UpdateArticle(Article updatedArticle)
        {
            var articles = GetAllArticles();
            var index = articles.FindIndex(a => a.Id == updatedArticle.Id);
            if (index != -1)
            {
                articles[index] = updatedArticle;
                SaveArticles(articles);
            }
        }

        public void DeleteArticle(string id)
        {
            var articles = GetAllArticles();
            articles.RemoveAll(a => a.Id == id);
            SaveArticles(articles);
        }

        private void SaveArticles(List<Article> articles)
        {
            var json = JsonSerializer.Serialize(articles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_articlesPath, json);
        }
    }
}
