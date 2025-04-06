using System.Text.Json;
using PersonalBlog.Models;
using PersonalBlog.Utilities;

namespace PersonalBlog.Services
{
    public class ArticleService
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _articlesDirectory = "Data/Articles";

        public ArticleService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            if (!_fileSystem.DirectoryExists(_articlesDirectory))
            {
                _fileSystem.CreateDirectory(_articlesDirectory);
            }
        }

        public List<Article> GetAllArticles()
        {
            var files = _fileSystem.GetFiles(_articlesDirectory, "*.json");
            var articles = new List<Article>();

            foreach (var file in files)
            {
                var json = _fileSystem.ReadAllText(file);
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
            _fileSystem.WriteAllText(filePath, json);
        }

        public void UpdateArticle(Article updatedArticle)
        {
            var filePath = Path.Combine(_articlesDirectory, $"{updatedArticle.Id}.json");
            if (!_fileSystem.FileExists(filePath)) return;

            var json = JsonSerializer.Serialize(updatedArticle, new JsonSerializerOptions { WriteIndented = true });
            _fileSystem.WriteAllText(filePath, json);
        }


        public void DeleteArticle(string id)
        {
            var filePath = Path.Combine(_articlesDirectory, $"{id}.json");
            if (_fileSystem.FileExists(filePath))
            {
                _fileSystem.DeleteFile(filePath);
            }
        }
    }
}
