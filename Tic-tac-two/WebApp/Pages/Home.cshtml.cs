using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Pages;

public class Home : PageModel
{
    AppDbContext _context;
    public string UserId { get; set; }
    public string Username { get; set; }
    
    public Home(AppDbContext context)
    {
        _context = context;
    }
    public void OnGet()
    {
        UserId = HttpContext.Session.GetString("UserId");
        Username = HttpContext.Session.GetString("Username");
    }

    public IActionResult OnPostConnect(string sessionId)
    {
        var session = _context.GameSessions.FirstOrDefault(s => s.Id == sessionId);
        if (session != null)
        {   
            session.Player2Id = HttpContext.Session.GetString("UserId");
            _context.SaveChanges();
            
            return RedirectToPage("/GameOnline", new { sessionId = sessionId });
        }
        return RedirectToPage("/Home");
        
    }
}