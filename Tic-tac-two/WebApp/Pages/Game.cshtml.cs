using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Game : PageModel
{
    [BindProperty(SupportsGet = true)] public string Mode { get; set; }
    [BindProperty(SupportsGet = true)] public string Config { get; set; }
    [BindProperty] public string? GameId { get; set; } = null;
    
    public Brain GameBrain { get; set; }
    private readonly ConfigRepositoryDb _configRepositoryDb;
    private readonly GameRepositoryDb _gameRepositoryDb;

    [BindProperty] public string Message { get; set; }
    [BindProperty] public bool disableBoard { get; set; }
    [BindProperty] public bool showOptions { get; set; }
    [BindProperty] public bool moveBoardOptions { get; set; }

    public Game(ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
    }
    
    
    public void OnGet(int? x, int? y, string gameId)
    {
        
        if (x == null || y == null || string.IsNullOrEmpty(gameId))
        {
            Console.WriteLine($"Mode: {Mode}, Config: {Config}, GameId: {GameId}");

            if (!string.IsNullOrEmpty(Config))
            {
                var config = _configRepositoryDb.GetConfigurationByName(Config);
                GameBrain = new Brain(config);
                GameId = Guid.NewGuid().ToString();
                GameBrain.SaveGame(GameId);
            }
            else if (!string.IsNullOrEmpty(GameId))
            {
                var gameState = _gameRepositoryDb.GetGameStateByName(GameId);
                GameBrain = new Brain(gameState);
                GameBrain.SaveGame(GameId);
            }
            else
            {
                RedirectToPage("/Error");
            }

            Message = $"Player {GameBrain?.playerNumber} is thinking";
        }
        
        else
        {
            Console.WriteLine($"GameId: {gameId}, x: {x}, y: {y}");

            var gameState = _gameRepositoryDb.GetGameStateByName(gameId);
            GameBrain = new Brain(gameState);
            GameBrain.placeChip(x.Value, y.Value);
            GameBrain.SaveGame(gameId);
        
            Message = $"Player {GameBrain?.playerNumber} is thinking";
        }
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




