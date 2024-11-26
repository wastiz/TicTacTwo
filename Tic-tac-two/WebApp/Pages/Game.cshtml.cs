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
    [BindProperty(SupportsGet = true)] public string? GameId { get; set; } = null;

    public Brain GameBrain { get; set; } = default!;
    private readonly ConfigRepositoryDb _configRepositoryDb;
    private readonly GameRepositoryDb _gameRepositoryDb;

    [BindProperty]
    public string Message { get; set; }

    [BindProperty]
    public bool player1MadeChoice { get; set; }

    [BindProperty]
    public bool player2MadeChoice { get; set; }

    [BindProperty]
    public bool disableBoard { get; set; }

    [BindProperty]
    public bool showOptions { get; set; }

    [BindProperty]
    public bool moveBoardOptions { get; set; }
    
    public GameState GameState { get; set; } = default!;

    public Game(Brain gameBrain, ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
        GameBrain = new Brain();
    }

    public void OnGet()
    {
        if (GameId != null)
        {
            // Загружаем состояние игры из базы данных по GameId
            GameState = _gameRepositoryDb.GetGameStateByName(GameId);

            // Инициализируем игру с состоянием, загруженным из базы данных
            GameBrain.Initialize(GameState);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
        else
        {
            GameId = Guid.NewGuid().ToString();
            var gameConfig = _configRepositoryDb.GetConfigurationByName(Config);
            GameBrain.Initialize(gameConfig);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
    }

    public IActionResult OnPostClick(int x, int y)
    {
        bool madeMove = GameBrain.placeChip(x, y);
        if (madeMove)
        {
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
        else
        {
            Message = "Sorry, you can't do this";
        }
        
        GameBrain.SaveGame(GameId);

        return Page();
    }

    public IActionResult OnPostOption(string option)
    {
        switch (option)
        {
            case "placeChip":
                break;
            case "moveBoard":
                moveBoardOptions = true;
                break;
            case "takeChip":
                break;
        }

        disableBoard = false;
        showOptions = false;
        return Page();
    }

    public IActionResult OnPostMoveBoard(string direction)
    {
        GameBrain.moveMovableBoard(direction);
        
        GameBrain.SaveGame(GameId);

        return Page();
    }

    private void CheckAndShowOptions()
    {
        if (GameBrain.chipsLeft[GameBrain.playerNumber] == 2 && 
            !(GameBrain.playerNumber == 1 ? player1MadeChoice : player2MadeChoice))
        {
            if (GameBrain.playerNumber == 1)
                player1MadeChoice = true;
            else
                player2MadeChoice = true;

            disableBoard = true;
            showOptions = true;
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



