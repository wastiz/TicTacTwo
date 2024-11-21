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
    
    private readonly Brain _gameBrain;
    private readonly ConfigRepositoryDb _configRepositoryDb;
    private readonly GameRepositoryDb _gameRepositoryDb;

    [BindProperty]
    public List<List<int>> Grid { get; set; }
    
    [BindProperty]
    public int BoardWidth { get; set; }
    [BindProperty]
    public int BoardHeight { get; set; }

    public Game(Brain gameBrain, ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        _gameBrain = gameBrain;
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
    }

    public void OnGet()
    {
        if (_gameBrain.board == null)
        {
            var gameConfig = _configRepositoryDb.GetConfigurationByName(Config);
            _gameBrain.Initialize(gameConfig);
            Grid = ConvertToList(_gameBrain.board);
            BoardWidth = _gameBrain.boardWidth;
            BoardHeight = _gameBrain.boardHeight;
        }
    }

    public void OnPostClick(int x, int y)
    {
        bool madeMove = _gameBrain.placeChip(x, y);
        Grid = ConvertToList(_gameBrain.board);
        BoardWidth = _gameBrain.boardWidth;
        BoardHeight = _gameBrain.boardHeight;
        RedirectToPage();
    }
    
    public List<List<int>> ConvertToList(int[,] matrix)
    {
        var list = new List<List<int>>();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            var row = new List<int>();
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                row.Add(matrix[i, j]);
            }
            list.Add(row);
        }
        return list;
    }
}


