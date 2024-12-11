using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Pages;

public class Home : PageModel
{
    AppDbContext _context;
    public Home(AppDbContext context)
    {
        _context = context;
    }
    public void OnGet()
    {
    }

    public IActionResult OnPostConnect(string sessionId)
    {
        bool sessionExists = _context.GameSessions.Any(s => s.Id == sessionId);
        Console.WriteLine($"Session Exists: {sessionExists}, SessionId: {sessionId}");
        if (sessionExists)
        {   
            return RedirectToPage("/GameOnline", new { sessionId = sessionId, connected = true });
        }
        return RedirectToPage("/Home");
        
    }
}