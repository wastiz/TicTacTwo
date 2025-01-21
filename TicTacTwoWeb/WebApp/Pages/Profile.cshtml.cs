using System.IdentityModel.Tokens.Jwt;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Profile : PageModel
{
    private UserRepository _userRepository;
    public string UserId { get; set; }
    [BindProperty] public string Username { get; set; }

    public Profile(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public void OnGet()
    {
        var token = HttpContext.Request.Cookies["authToken"];

        if (!string.IsNullOrEmpty(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            Username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
        }
    }
    
    public IActionResult OnPostLogOut()
    {
        HttpContext.Response.Cookies.Delete("authToken");
        return RedirectToPage("/Index");
    }
    public IActionResult OnPostDeleteAccount()
    {
        _userRepository.DeleteUser(UserId);
        return RedirectToPage("/Index");
    }

}