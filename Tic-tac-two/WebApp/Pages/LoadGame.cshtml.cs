using DAL;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class LoadGame : PageModel
{
    private AppDbContext _context;
    [BindProperty] public string SessionId { get; set; }
    public List<GameSessionDto> Games { get; set; }


    public LoadGame(AppDbContext context)
    {
        _context = context;
    }
    
    public void OnGet()
    {
        Games = _context.GameSessions
            .Include(gs => gs.GameState)
            .Include(gs => gs.Player1)
            .Include(gs => gs.Player2)
            .Select(gs => new GameSessionDto
            {
                SessionId = gs.Id,
                StateName = gs.GameState.Name
            })
            .ToList();
    }

    public IActionResult OnPost()
    {
        if (SessionId != null)
        {
            return RedirectToPage("/Game", new { sessionId = SessionId });
        }
        return Page();
    }

}






