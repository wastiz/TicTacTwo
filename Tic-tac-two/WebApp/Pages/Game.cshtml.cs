using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Game : PageModel
{
    
    [BindProperty]
    public string Mode { get; set; }
    [BindProperty]
    public string Config { get; set; }
    
    public Brain _gameBrain;
    public ConfigRepositoryDb _configRepositoryDb;
    public GameRepositoryDb _gameRepositoryDb;

    public Game()
    {
        _configRepositoryDb = new ConfigRepositoryDb();
        _gameRepositoryDb = new GameRepositoryDb();
        GameConfiguration config = _configRepositoryDb.GetConfigurationByName(Config);
        _gameBrain = new Brain(config);
    }
    public void OnGet()
    {
    }
    
    
}