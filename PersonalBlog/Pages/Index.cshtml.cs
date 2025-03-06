using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBlog.Models;
using PersonalBlog.Services;
namespace PersonalBlog.Pages 
{ 
    public class IndexModel : PageModel
    {
        private readonly ArticleService _articleService;

        public List<Models.Article> Articles { get; set; } = new List<Models.Article>();

        public IndexModel(ArticleService articleService)
        {
            _articleService = articleService;
        }

        public void OnGet()
        {
            Articles = _articleService.GetAllArticles();
        }
    }
}