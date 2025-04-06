using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Moq;
using NUnit.Framework;
using PersonalBlog.Models;
using PersonalBlog.Services;
using PersonalBlog.Utilities;

namespace PersonalBlog.Tests.Services
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private Mock<IFileSystem> _fileSystemMock;
        private ArticleService _articleService;

        [SetUp]
        public void SetUp()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _fileSystemMock.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(fs => fs.CreateDirectory(It.IsAny<string>()));
            _fileSystemMock.Setup(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
            _articleService = new ArticleService(_fileSystemMock.Object);
        }

        [Test]
        public void GetAllArticles_ReturnsOrderedArticles()
        {
            // Arrange
            var articles = new List<Article>
            {
                new Article { Id = "a0ac9a26-568b-4240-8989-662b7058f3ba", Title = "Article 1", PublishedDate = new DateTime(2023, 1, 1) },
                new Article { Id = "b1bd9b37-679c-5351-909a-773c8169f4cb", Title = "Article 2", PublishedDate = new DateTime(2023, 2, 1) }
            };
            var files = new[] { "a0ac9a26-568b-4240-8989-662b7058f3ba.json", "b1bd9b37-679c-5351-909a-773c8169f4cb.json" };
            _fileSystemMock.Setup(fs => fs.GetFiles(It.IsAny<string>(), "*.json")).Returns(files);
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns<string>(path =>
            {
                var id = Path.GetFileNameWithoutExtension(path);
                return JsonSerializer.Serialize(articles.Find(a => a.Id == id));
            });

            // Act
            var result = _articleService.GetAllArticles();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Id, Is.EqualTo("b1bd9b37-679c-5351-909a-773c8169f4cb"));
            Assert.That(result[1].Id, Is.EqualTo("a0ac9a26-568b-4240-8989-662b7058f3ba"));
        }

        [Test]
        public void GetArticleById_ReturnsArticle()
        {
            // Arrange
            var article = new Article { Id = "a0ac9a26-568b-4240-8989-662b7058f3ba", Title = "Article 1" };
            var filePath = Path.Combine("Data/Articles", "a0ac9a26-568b-4240-8989-662b7058f3ba.json");
            _fileSystemMock.Setup(fs => fs.FileExists(filePath)).Returns(true);
            _fileSystemMock.Setup(fs => fs.ReadAllText(filePath)).Returns(JsonSerializer.Serialize(article));

            // Act
            var result = _articleService.GetArticleById("a0ac9a26-568b-4240-8989-662b7058f3ba");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("a0ac9a26-568b-4240-8989-662b7058f3ba"));
        }

        [Test]
        public void AddArticle_CreatesFile()
        {
            // Arrange
            var article = new Article { Id = "a0ac9a26-568b-4240-8989-662b7058f3ba", Title = "Article 1" };
            var filePath = Path.Combine("Data/Articles", "a0ac9a26-568b-4240-8989-662b7058f3ba.json");

            // Act
            _articleService.AddArticle(article);

            // Assert
            _fileSystemMock.Verify(fs => fs.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UpdateArticle_UpdatesFile()
        {
            // Arrange
            var article = new Article { Id = "a0ac9a26-568b-4240-8989-662b7058f3ba", Title = "Updated Article" };
            var filePath = Path.Combine("Data/Articles", "a0ac9a26-568b-4240-8989-662b7058f3ba.json");
            _fileSystemMock.Setup(fs => fs.FileExists(filePath)).Returns(true);

            // Act
            _articleService.UpdateArticle(article);

            // Assert
            _fileSystemMock.Verify(fs => fs.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void DeleteArticle_DeletesFile()
        {
            // Arrange
            var filePath = Path.Combine("Data/Articles", "a0ac9a26-568b-4240-8989-662b7058f3ba.json");
            _fileSystemMock.Setup(fs => fs.FileExists(filePath)).Returns(true);

            // Act
            _articleService.DeleteArticle("a0ac9a26-568b-4240-8989-662b7058f3ba");

            // Assert
            _fileSystemMock.Verify(fs => fs.DeleteFile(filePath), Times.Once);
        }
    }
}
