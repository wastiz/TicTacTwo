using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly UserRepository _userRepository;

    public RegisterModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
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
        var (created, message) = _userRepository.CreateUser(Username, Password);
        if (created)
        {
            Message = message;
            return RedirectToPage("/Index");
        }
        Message = message;
        return Page();
    }
}