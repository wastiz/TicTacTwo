using System.IdentityModel.Tokens.Jwt;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Home : PageModel
{
    SessionRepository _sessionRepository;
    public string UserId { get; set; }
    [BindProperty] public string Username { get; set; }
    
    
    public Home(SessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
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

    public IActionResult OnPostConnect(string sessionId)
    {
        var session = _sessionRepository.GetSessionById(sessionId);
        if (session != null)
        {   
            _sessionRepository.SaveSecondPlayer(session, UserId);
            
            return RedirectToPage("/GameOnline", new { sessionId = sessionId });
        }
        return RedirectToPage("/Home");
        
    }
}