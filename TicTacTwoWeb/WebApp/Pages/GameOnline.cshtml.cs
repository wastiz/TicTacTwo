using System.IdentityModel.Tokens.Jwt;
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
        public string UserId { get; set; }
        [BindProperty] public string Username { get; set; }

        public GameOnline(SessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public void InitializeGame()
        {
            Session = _sessionRepository.GetSessionById(SessionId);
            if (Session == null)
            {
                throw new System.Exception("Game session not found.");
            }

            GameBrain = new Brain(Session.GameConfiguration, Session.GameState);
            ViewData["userNumber"] = Session.Player1Id == UserId ? 1 : 2;
        }

        public void OnGet()
        {
            var token = HttpContext.Request.Cookies["authToken"];

            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                Username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            }
            
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
