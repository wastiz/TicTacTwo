using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;

    public RegisterModel(AppDbContext context)
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
        if (_context.Users.Any(u => u.Username == Username))
        {
            Message = "This username is already taken.";
            return Page();
        }

        var newUser = new User { Username = Username, Password = Password };
        _context.Users.Add(newUser);
        _context.SaveChanges();
        Message = "Account created successfully!";
        return RedirectToPage("/Index");
    }
}