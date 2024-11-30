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
    public int[]? replacingChip { get; set; } = null;
    public bool replacing { get; set; } = false;
    
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

    [HttpPost]
    public IActionResult OnPostClick([FromBody] MoveRequest request)
    {
        var gameState = _gameRepositoryDb.GetGameStateByName(request.GameId);
        if (gameState == null)
        {
            return BadRequest("Game not found.");
        }
        
        GameBrain = new Brain(gameState);

        bool madeMove;
        string message;

        if (replacingChip == null && !replacing)
        {
            if ((GameBrain.player1Options && GameBrain.playerNumber == 1 && GameBrain.board[request.X, request.Y] == 1) ||
                (GameBrain.player2Options && GameBrain.playerNumber == 2 && GameBrain.board[request.X, request.Y] == 2))
            {
                replacingChip = new[] { request.X, request.Y };
                replacing = true;
                message = "Place your piece where you want";
            }
            else
            {
                madeMove = GameBrain.placeChip(request.X, request.Y);
                message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here";
            }
        }
        else
        {
            madeMove = GameBrain.moveChip(replacingChip[0], replacingChip[1], request.X, request.Y);
            replacingChip = null;
            replacing = false;
            message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here";
        }
        
        GameBrain.SaveGame(request.GameId);
        
        return new JsonResult(new { message, board = ConvertToList(GameBrain.board) });
    }

    public class MoveRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string GameId { get; set; }
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




