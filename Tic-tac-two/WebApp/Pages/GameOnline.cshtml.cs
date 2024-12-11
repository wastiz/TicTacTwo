using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace WebApp.Pages
{
    public class GameOnline : PageModel
    {
        [BindProperty(SupportsGet = true)] public string SessionId { get; set; }
        [BindProperty(SupportsGet = true)] public bool Connected { get; set; }
        public GameSessionDB Session;
        public string StateId;
        public Brain GameBrain { get; set; }
        private readonly GameRepositoryDb _gameRepositoryDb;
        private AppDbContext _context;
        [BindProperty] public string Message { get; set; }
        public string CurrentPlayer { get; set; }

        public GameOnline(AppDbContext context, GameRepositoryDb gameRepositoryDb)
        {
            _gameRepositoryDb = gameRepositoryDb;
            _context = context;
        }

        public void OnGet()
        {
            // Load session and game state
            Session = _context.GameSessions.FirstOrDefault(s => s.Id == SessionId);

            if (Session == null)
            {
                Message = "Game session not found!";
                return;
            }

            if (Connected && Session.Player2Id == null)
            {
                if (TempData["UserId"] != null)
                {
                    Session.Player2Id = TempData["UserId"].ToString();
                    CurrentPlayer = Session.Player1Id;
                    _context.SaveChanges();
                }
                else
                {
                    Message = "User ID not found in TempData!";
                    return;
                }
            }

            StateId = Session.GameStateId;
            var gameState = _gameRepositoryDb.GetGameStateById(StateId);
            GameBrain = new Brain(gameState);
        }

        // Handling chip placement
        public class PlaceChipRequest
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string GameId { get; set; }
            public int PlayerNumber { get; set; }
        }

        [HttpPost]
        public IActionResult OnPostClick([FromBody] PlaceChipRequest request)
        {
            var gameState = _gameRepositoryDb.GetGameStateById(request.GameId);
            GameBrain = new Brain(gameState);

            // Ensure it's the player's turn
            if (GameBrain.playerNumber != request.PlayerNumber)
            {
                return new JsonResult(new { message = "It's not your turn!" });
            }

            bool madeMove = GameBrain.placeChip(request.X, request.Y);

            if (madeMove)
            {
                GameBrain.SaveGame(request.GameId);
            }

            return new JsonResult(new
            {
                message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here",
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY
            });
        }

        // Handling chip movement
        public class MoveChipRequest
        {
            public int StartX { get; set; }
            public int StartY { get; set; }
            public int EndX { get; set; }
            public int EndY { get; set; }
            public string GameId { get; set; }
        }

        public IActionResult OnPostMoveChip([FromBody] MoveChipRequest request)
        {
            var gameState = _gameRepositoryDb.GetGameStateById(request.GameId);
            GameBrain = new Brain(gameState);

            // Ensure it's the player's turn
            if (GameBrain.playerNumber != request.StartX)
            {
                return new JsonResult(new { message = "It's not your turn!" });
            }

            bool madeMove = GameBrain.moveChip(request.StartX, request.StartY, request.EndX, request.EndY);
            string message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "Invalid move";

            GameBrain.SaveGame(request.GameId);

            return new JsonResult(new
            {
                message,
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                player1Options = GameBrain.player1Options,
                player2Options = GameBrain.player2Options,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY,
            });
        }

        // Handling movable board moves
        public class MoveBoardRequest
        {
            public string Direction { get; set; }
            public string GameId { get; set; }
        }

        public IActionResult OnPostMoveBoard([FromBody] MoveBoardRequest request)
        {
            GameBrain = new Brain(_gameRepositoryDb.GetGameStateById(request.GameId));
            string message;

            bool madeMove = GameBrain.moveMovableBoard(request.Direction);
            if (madeMove)
            {
                GameBrain.SaveGame(request.GameId);
                message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't move there";
            }
            else
            {
                message = "Invalid move";
            }

            return new JsonResult(new
            {
                message,
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                player1Options = GameBrain.player1Options,
                player2Options = GameBrain.player2Options,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY,
            });
        }

        // Convert game board array to list
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

        // Save game name
        public IActionResult OnPostSaveName(string gameId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToPage("/NewGame");
            }

            _gameRepositoryDb.SaveStateName(StateId, name);

            return RedirectToPage("/NewGame");
        }

        // Handle game state fetch
        public IActionResult OnPostGetState([FromBody] PlaceChipRequest request)
        {
            var gameState = _gameRepositoryDb.GetGameStateById(request.GameId);
            GameBrain = new Brain(gameState);

            return new JsonResult(new
            {
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                currentPlayerId = GameBrain.playerNumber == 1 ? Session.Player1Id : Session.Player2Id,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY
            });
        }
    }
}