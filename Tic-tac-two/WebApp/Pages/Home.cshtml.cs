using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Pages;

public class Home : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Username { get; set; }
    public IActionResult OnGet()
    {
        TempData["Message"] = "Firstly you have to log in or create a new account.";
        if (Username.IsNullOrEmpty()) return RedirectToPage("/Index");
        return Page();
    }
}