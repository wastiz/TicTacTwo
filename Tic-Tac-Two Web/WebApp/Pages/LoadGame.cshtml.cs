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
    public List<GameSessionDto> Games { get; set; }


    public LoadGame(SessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
    
    public void OnGet()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        Games = _sessionRepository.GetSessionDtos();
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






