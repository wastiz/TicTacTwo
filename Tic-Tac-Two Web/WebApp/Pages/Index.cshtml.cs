using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using DAL;

public class IndexModel : PageModel
{
    private readonly UserRepository _userRepository;
    private readonly JwtTokenHelper _jwtTokenHelper;
    [BindProperty] public string Username { get; set; }
    [BindProperty] public string Password { get; set; }
    public string Message { get; set; }

    public IndexModel(UserRepository userRepository, JwtTokenHelper jwtTokenHelper)
    {
        _userRepository = userRepository;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public IActionResult OnGet()
    {
        var token = HttpContext.Request.Cookies["authToken"];
        if (!string.IsNullOrEmpty(token))
        {
            RedirectToPage("/Home");
        }

        if (User.Identity.IsAuthenticated)
        {
            RedirectToPage("/Home");
        }
        return Page();
    }

    public IActionResult OnPost([FromBody] LoginRequest request)
    {
        var (logged, message, user) = _userRepository.Login(request.Username, request.Password);

        if (!logged)
        {
            Message = message;
            return Page();  
        }

        var token = _jwtTokenHelper.GenerateToken(user.Id.ToString(), user.Username);
        
        return new JsonResult(new { success = true, token, userId = user.Id });
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}