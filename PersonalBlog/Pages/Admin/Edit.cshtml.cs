using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBlog.Models;
using PersonalBlog.Services;

namespace PersonalBlog.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly ArticleService _articleService;

        public EditModel(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [BindProperty]
        public Article Article { get; set; } = new();

        public IActionResult OnGet(string id)
        {
            if (!HttpContext.Session.GetString("IsAdmin")?.Equals("true") ?? true)
            {
                return RedirectToPage("/Login");
            }

            var existingArticle = _articleService.GetArticleById(id);
            if (existingArticle == null)
            {
                return NotFound();
            }

            Article = existingArticle;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _articleService.UpdateArticle(Article);
            return RedirectToPage("/Admin/Dashboard");
        }

    }
}
