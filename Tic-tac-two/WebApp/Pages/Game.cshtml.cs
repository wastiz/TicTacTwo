using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Game : PageModel
{
    [BindProperty(SupportsGet = true)] public string GameId { get; set; }
    
    public Brain GameBrain { get; set; }
    private readonly ConfigRepositoryDb _configRepositoryDb;
    private readonly GameRepositoryDb _gameRepositoryDb;

    [BindProperty] public string Message { get; set; }

    public Game(ConfigRepositoryDb configRepositoryDb, GameRepositoryDb gameRepositoryDb)
    {
        _configRepositoryDb = configRepositoryDb;
        _gameRepositoryDb = gameRepositoryDb;
    }
    
    
    public void OnGet(int? x, int? y, string gameId)
    {
        
        if (x == null || y == null || string.IsNullOrEmpty(gameId))
        {
            Console.WriteLine($"GameId: {GameId}");
            
            if (!string.IsNullOrEmpty(GameId))
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
    }

    public IActionResult OnPostClick(int x, int y, string gameId)
    {
        
        var gameState = _gameRepositoryDb.GetGameStateByName(gameId);
        GameBrain = new Brain(gameState);

        if (GameBrain.player1Options && x == 1 && y == 1 && GameBrain.playerNumber == 1)
        {
            Message = "Place your piece where you want";
        }
        
        bool madeMove = GameBrain.placeChip(x, y);
        if (madeMove)
        {
            GameBrain.SaveGame(gameId);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
        else
        {
            Message = "You can't place here";
        }
        
        return Page();
    }
    
    public IActionResult OnPostMoveBoard(string direction, string gameId)
    {
        GameBrain = new Brain(_gameRepositoryDb.GetGameStateByName(gameId));
        
        Console.WriteLine(GameBrain.playerNumber);
        bool madeMove = GameBrain.moveMovableBoard(direction);
        if (madeMove)
        {
            GameBrain.SaveGame(GameId);
            Console.WriteLine(GameBrain.playerNumber);
        }
        else
        {
            Message = "You can't move there";
        }
        

        return Page();
    }
    
    public IActionResult OnPostMovePiece(int sourceX, int sourceY, int targetX, int targetY, string gameId)
    {
        GameBrain = new Brain(_gameRepositoryDb.GetGameStateByName(gameId));
    
        bool moved = GameBrain.moveChip(sourceX, sourceY, targetX, targetY);
        if (moved)
        {
            GameBrain.SaveGame(gameId);
            Message = $"Player {GameBrain.playerNumber} moved a piece.";
        }
        else
        {
            Message = "Invalid move.";
        }
    
        return Page();
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




