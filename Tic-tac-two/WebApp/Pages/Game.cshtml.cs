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
    
    public Brain GameBrain { get; set; } = default!;
    private readonly ConfigRepositoryDb _configRepositoryDb;
    private readonly GameRepositoryDb _gameRepositoryDb;
    
    [BindProperty]
    public String Message { get; set; }
    
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

    public Game(Brain gameBrain, ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        GameBrain = gameBrain;
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
    }

    public void OnGet()
    {
        if (GameBrain.board == null)
        {
            var gameConfig = _configRepositoryDb.GetConfigurationByName(Config);
            GameBrain.Initialize(gameConfig);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
    }

    public IActionResult OnPostClick(int x, int y)
    {
        bool playerMadeChoice = GameBrain.playerNumber == 1 ? player1MadeChoice : player2MadeChoice;
        if (GameBrain.chipsLeft[GameBrain.playerNumber] == 2 && !playerMadeChoice)
        {
            if (GameBrain.playerNumber == 1)
                player1MadeChoice = true;
            else 
                player2MadeChoice = true;

            disableBoard = true;
            showOptions = true;
        }
        else
        {
            PlaceChip(x, y);
        }
        return Page();
    }

    public IActionResult OnPostOption(String option)
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

    public IActionResult OnPostMoveBoard(String direction)
    {
        GameBrain.moveMovableBoard(direction);
        return Page();
    }

    public void PlaceChip(int x, int y)
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


