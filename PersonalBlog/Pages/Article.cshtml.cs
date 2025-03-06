using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBlog.Models;
using PersonalBlog.Services;

namespace PersonalBlog.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly ArticleService _articleService;

        public ArticleModel(ArticleService articleService)
        {
            _articleService = articleService;
        }

        public Article? Article { get; set; }

        public IActionResult OnGet(string id)
        {
            Article = _articleService.GetArticleById(id);

            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
