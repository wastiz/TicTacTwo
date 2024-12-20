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
        private readonly GameRepositoryDb _gameRepositoryDb;
        private AppDbContext _context;
        public GameSessionDB Session;
        public string StateId;
        public Brain GameBrain { get; set; }
        public string UserId;
        public string Username;
        public bool isPlayerTurn;
        [BindProperty] public string Message { get; set; }

        public GameOnline(AppDbContext context, GameRepositoryDb gameRepositoryDb)
        {
            _gameRepositoryDb = gameRepositoryDb;
            _context = context;
        }

        public void CheckYourTurn()
        {
            isPlayerTurn = (UserId == Session.Player1Id && GameBrain.playerNumber == 1) || (UserId == Session.Player2Id && GameBrain.playerNumber == 2);
        }
        public void InitializeGame()
        {
            Session = _context.GameSessions.FirstOrDefault(s => s.Id == SessionId);
            StateId = Session.GameStateId;
            var gameState = _gameRepositoryDb.GetGameStateById(StateId);
            GameBrain = new Brain(gameState);
            UserId = HttpContext.Session.GetString("UserId");
            Username = HttpContext.Session.GetString("Username");
            CheckYourTurn();
        }
        
        public IActionResult OnGetGameState()
        {
            InitializeGame();

            return new JsonResult(new
            {
                success = true,
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                player1Options = GameBrain.player1Options,
                player2Options = GameBrain.player2Options,
                isYourTurn = isPlayerTurn,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY
            });
        }
        
        public void OnGet()
        {
            InitializeGame();
        }
        
        public class PlaceChipRequest
        {
            public int X { get; set; }
            public int Y { get; set; }
            
        }

        [HttpPost]
        public IActionResult OnPostClick([FromBody] PlaceChipRequest request)
        {
            InitializeGame();
            if (isPlayerTurn)
            {
                bool madeMove = GameBrain.placeChip(request.X, request.Y);

                if (madeMove)
                {
                    GameBrain.SaveGame(StateId);
                }
                CheckYourTurn();
                return new JsonResult(new
                {
                    message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here",
                    board = ConvertToList(GameBrain.board),
                    win = GameBrain.win,
                    isYourTurn = isPlayerTurn,
                    gridX = GameBrain.gridX,
                    gridY = GameBrain.gridY
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "It's not your turn!",
                });
            }   
        }
        }
        
        public class MoveChipRequest
        {
            public int StartX { get; set; }
            public int StartY { get; set; }
            public int EndX { get; set; }
            public int EndY { get; set; }
        }

        public IActionResult OnPostMoveChip([FromBody] MoveChipRequest request)
        {
            InitializeGame();

            bool madeMove = GameBrain.moveChip(request.StartX, request.StartY, request.EndX, request.EndY);
            string message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "Invalid move";

            GameBrain.SaveGame(StateId);
            
            CheckYourTurn();

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
        
        public class MoveBoardRequest
        {
            public string Direction { get; set; }
        }

        public IActionResult OnPostMoveBoard([FromBody] MoveBoardRequest request)
        {
            InitializeGame();
            string message;

            bool madeMove = GameBrain.moveMovableBoard(request.Direction);
            if (madeMove)
            {
                GameBrain.SaveGame(StateId);
                message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't move there";
            }
            else
            {
                message = "Invalid move";
            }
            
            CheckYourTurn();

            return new JsonResult(new
            {
                message,
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                player1Options = GameBrain.player1Options,
                player2Options = GameBrain.player2Options,
                isYourTurn = isPlayerTurn,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY,
            });
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
        
        public IActionResult OnPostSaveName(string gameId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToPage("/NewGame");
            }

            _gameRepositoryDb.SaveStateName(StateId, name);

            return RedirectToPage("/NewGame");
        }

    }
}