using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Game : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Mode { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string Config { get; set; }

    public Brain _gameBrain;
    public ConfigRepositoryDb _configRepositoryDb;
    public GameRepositoryDb _gameRepositoryDb;

    [BindProperty]
    public int GridWidth { get; set; }
    [BindProperty]
    public int GridHeight { get; set; }
    [BindProperty]
    public int MovableGridWidth { get; set; }
    [BindProperty]
    public int MovableGridHeight { get; set; }
    public int[,] Grid { get; set; }

    public Game(ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
    }

    public void OnGet()
    {
        if (!string.IsNullOrEmpty(Config))
        {
            InitializeGame();
        }
    }

    public IActionResult OnPost(string config, string mode)
    {
        Config = config;
        Mode = mode;
        
        InitializeGame();
        
        return Page();
    }

    private void InitializeGame()
    {
        var gameConfig = _configRepositoryDb.GetConfigurationByName(Config);
        
        _gameBrain = new Brain(gameConfig);
        
        GridWidth = _gameBrain.boardWidth;
        GridHeight = _gameBrain.boardHeight;
        MovableGridWidth = _gameBrain.movableBoardWidth;
        MovableGridHeight = _gameBrain.movableBoardHeight;
        Grid = _gameBrain.board;
    }
}
