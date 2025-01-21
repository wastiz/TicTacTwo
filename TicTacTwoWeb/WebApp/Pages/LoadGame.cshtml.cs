using System.IdentityModel.Tokens.Jwt;
using DAL;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class LoadGame : PageModel
{
    private SessionRepository _sessionRepository;
    [BindProperty] public string SessionId { get; set; }
    public string UserId { get; set; }
    [BindProperty] public string Username { get; set; }
    public List<GameSessionDto> Games { get; set; }


    public LoadGame(SessionRepository sessionRepository)
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
        
        Games = _sessionRepository.GetUserSessionDto(UserId);
    }

    public IActionResult OnPost()
    {
        if (SessionId != null)
        {
            return RedirectToPage("/Game", new { sessionId = SessionId });
        }
        return Page();
    }
    
    public IActionResult OnPostDelete([FromBody] DeleteRequest request)
    {
        _sessionRepository.DeleteSession(request.SessionId);
        return new JsonResult(new { success = true });
    }
    
    public class DeleteRequest
    {
        public string SessionId { get; set; }
    }
}






