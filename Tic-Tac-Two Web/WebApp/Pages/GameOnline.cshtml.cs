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
        private readonly SessionRepository _sessionRepository;
        public GameSession Session;
        public Brain GameBrain { get; set; }
        public string UserId;
        public bool isPlayerTurn;
        public string Message { get; set; }

        public GameOnline(SessionRepository gameRepository)
        {
            _sessionRepository = gameRepository;
        }

        public void InitializeGame()
        {
            Session = _sessionRepository.GetSessionById(SessionId);
            if (Session == null)
            {
                throw new System.Exception("Game session not found.");
            }

            GameBrain = new Brain(Session.GameConfiguration, Session.GameState);
            UserId = HttpContext.Session.GetString("UserId");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
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
        
        public IActionResult OnPostSaveName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {   
                return RedirectToPage("/NewGame");
            }
        
            _sessionRepository.SaveSessionName(SessionId, name);
        
            return RedirectToPage("/NewGame");
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
