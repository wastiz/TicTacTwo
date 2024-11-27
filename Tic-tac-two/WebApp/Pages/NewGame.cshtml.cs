using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    private ConfigRepositoryDb _configRepository;
    private GameRepositoryDb _gameRepository;

    public NewGame(AppDbContext context)
    {
        _configRepository = new ConfigRepositoryDb();
        _gameRepository = new GameRepositoryDb();
    }
    
    public List<string> GameConfigs { get; set; } = new List<string>();
    public void OnGet() 
    {
        GameConfigs = _configRepository.GetAllConfigNames();
    }
    [BindProperty] public string GameMode { get; set; }
    [BindProperty] public string GameConfig { get; set; }
    public string GameId { get; set; }

    public IActionResult OnPost()
    {
        Brain gameBrain = new Brain(_configRepository.GetConfigurationByName(GameConfig));
        GameId = Guid.NewGuid().ToString();
        gameBrain.SaveGame(GameId);
        return RedirectToPage("/Game", new { gameId = GameId });
    }
}