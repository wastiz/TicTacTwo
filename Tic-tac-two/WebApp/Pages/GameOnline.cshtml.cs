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
        private readonly AppDbContext _context;
        public GameSessionDB Session;
        public string StateId;
        public Brain GameBrain { get; set; }
        public string UserId;
        public string Username;
        public bool isPlayerTurn;
        public string Message { get; set; }

        public GameOnline(AppDbContext context, GameRepositoryDb gameRepositoryDb)
        {
            _gameRepositoryDb = gameRepositoryDb;
            _context = context;
        }

        public void InitializeGame()
        {
            Session = _context.GameSessions.FirstOrDefault(s => s.Id == SessionId);
            if (Session == null)
            {
                throw new System.Exception("Game session not found.");
            }

            StateId = Session.GameStateId;
            var gameState = _gameRepositoryDb.GetGameStateById(StateId);
            if (gameState == null)
            {
                throw new System.Exception("Game state not found.");
            }

            GameBrain = new Brain(gameState);
            UserId = HttpContext.Session.GetString("UserId");
            Username = HttpContext.Session.GetString("Username");
            ViewData["userNumber"] = Session.Player1Id == UserId ? 1 : 2;
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
}
