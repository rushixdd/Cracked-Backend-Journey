using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBlog.Models;
using PersonalBlog.Services;
using System;

namespace PersonalBlog.Pages.Admin
{
    public class AddModel : PageModel
    {
        private readonly ArticleService _articleService;

        public AddModel(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [BindProperty]
        public Article Article { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!HttpContext.Session.GetString("IsAdmin")?.Equals("true") ?? true)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Article.Id = Guid.NewGuid().ToString(); // Generate a unique ID
            Article.PublishedDate = DateTime.Now; // Set publication date
            _articleService.AddArticle(Article);

            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
