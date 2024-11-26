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
    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; }
    [BindProperty(SupportsGet = true)] public string? GameId { get; set; } = null;

    public Brain GameBrain { get; set; }
    private readonly ConfigRepositoryDb _configRepositoryDb;
    private readonly GameRepositoryDb _gameRepositoryDb;

    [BindProperty] public string Message { get; set; }
    [BindProperty] public bool player1MadeChoice { get; set; }
    [BindProperty] public bool player2MadeChoice { get; set; }
    [BindProperty] public bool disableBoard { get; set; }
    [BindProperty] public bool showOptions { get; set; }
    [BindProperty] public bool moveBoardOptions { get; set; }

    public Game(ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
        if (!string.IsNullOrEmpty(ConfigName))
        {
            GameBrain = new Brain(_configRepositoryDb.GetConfigurationByName(ConfigName));
            GameId = Guid.NewGuid().ToString();
            GameBrain.SaveGame(GameId);
        }

        if (!string.IsNullOrEmpty(GameId))
        {
            GameBrain = new Brain(_gameRepositoryDb.GetGameStateByName(GameId));
            GameBrain.SaveGame(GameId);
        }
    }

    public void OnGet()
    {
        // Проверяем, что GameBrain не равен null, если это необходимо
        if (GameBrain == null)
        {
            if (!string.IsNullOrEmpty(ConfigName))
            {
                GameBrain = new Brain(_configRepositoryDb.GetConfigurationByName(ConfigName));
                GameId = Guid.NewGuid().ToString();
                GameBrain.SaveGame(GameId);
            }
            else if (!string.IsNullOrEmpty(GameId))
            {
                GameBrain = new Brain(_gameRepositoryDb.GetGameStateByName(GameId));
                GameBrain.SaveGame(GameId);
            }
        }
    
        // Теперь безопасно обращаемся к свойству GameBrain
        Message = $"Player {GameBrain?.playerNumber} is thinking";
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





