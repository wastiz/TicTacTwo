using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using DAL;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    public void OnGet()
    {
       
    }

    public IActionResult OnPost()
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Username == Username);

        if (existingUser == null)
        {
            Message = "User not found!";
            return Page();
        }

        if (existingUser.Password != Password)
        {
            Message = "Wrong username/password!";
            return Page();
        }

        Message = "Welcome!";
        HttpContext.Session.SetString("UserId", existingUser.Id);
        HttpContext.Session.SetString("Username", existingUser.Username);
        return RedirectToPage("/Home");
    }

}