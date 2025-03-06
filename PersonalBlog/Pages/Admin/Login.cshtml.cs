using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonalBlog.Pages.Admin
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public required string Username { get; set; }

        [BindProperty]
        public required string Password { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            HttpContext.Session.Clear();
        }

        public IActionResult OnPost()
        {
            const string adminUsername = "admin";
            const string adminPassword = "password123";

            if (Username == adminUsername && Password == adminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToPage("/Admin/Dashboard");
            }
            else
            {
                ErrorMessage = "Invalid username or password!";
                return Page();
            }
        }
    }
}
