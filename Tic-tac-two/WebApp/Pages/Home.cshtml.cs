using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Home : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string UserName { get; set; }
    
    public IActionResult OnGet()
    {
        TempData["Message"] = "Firstly log in or create account!";
        if (string.IsNullOrEmpty(UserName)) return RedirectToPage("/Index");
        return Page();
    }
}