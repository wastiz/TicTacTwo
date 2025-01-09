using DAL;
using DAL.DTO;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    private ConfigRepository _configRepository;
    private SessionRepository _sessionRepository;
    [BindProperty] public string GameMode { get; set; }
    [BindProperty] public string ConfigId { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public List<GameConfigDto> GameConfigs { get; set; }

    public NewGame(ConfigRepository configRepository, SessionRepository sessionRepository)
    {
        _configRepository = configRepository;
        _sessionRepository = sessionRepository;
    }

    public void OnGet()
    {
        UserId = HttpContext.Session.GetString("UserId");
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        GameConfigs = _configRepository.GetAllConfigDto();
    }

    public IActionResult OnPost()
    {
        var session = _sessionRepository.CreateGameSession(_configRepository.GetConfigurationById(ConfigId));
            
        if (GameMode == "two-players")
        {
            return RedirectToPage("/Game" , new { sessionId = session.Id });
        } else if (GameMode == "two-players-online")
        {
            return RedirectToPage("/GameOnline", new { sessionId = session.Id });
        }
        return Page();
    }
}