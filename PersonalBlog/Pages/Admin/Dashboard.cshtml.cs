using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBlog.Models;
using PersonalBlog.Services;
using System.Collections.Generic;

namespace PersonalBlog.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ArticleService _articleService;

        public DashboardModel(ArticleService articleService)
        {
            _articleService = articleService;
        }

        public List<PersonalBlog.Models.Article> Articles { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!HttpContext.Session.GetString("IsAdmin")?.Equals("true") ?? true)
            {
                return RedirectToPage("/Login");
            }

            Articles = _articleService.GetAllArticles();
            return Page();
        }

        public IActionResult OnPostDelete(string id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToPage();
        }
    }
}
