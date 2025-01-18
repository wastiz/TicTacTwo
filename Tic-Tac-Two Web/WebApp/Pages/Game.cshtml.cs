using System.IdentityModel.Tokens.Jwt;
using DAL;
using GameBrain;

namespace WebApp.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Game : PageModel
{
    private readonly SessionRepository _sessionRepository;
    [BindProperty(SupportsGet = true)] public string SessionId { get; set; }
    public GameSession Session;
    public Brain GameBrain { get; set; }
    [BindProperty] public string Message { get; set; }
    [BindProperty] public string Username { get; set; }
    [BindProperty] public string UserId { get; set; }

    public Game(SessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
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
        
        var (config, state) = _sessionRepository.GetGameState(SessionId);
        GameBrain = new Brain(config, state);
    }

    
    public class PlaceChipRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string SessionId { get; set; }
    }

    [HttpPost]
    public IActionResult OnPostClick([FromBody] PlaceChipRequest request)
    {
        SessionId = request.SessionId;
        var (config, state) = _sessionRepository.GetGameState(SessionId);
        GameBrain = new Brain(config, state);

        bool madeMove = GameBrain.placeChip(request.X, request.Y);
        if (madeMove)
        {
            GameBrain.SaveGame(SessionId);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
        else
        {
            Message = "You can't place here";
        }

        return new JsonResult(new
        {
            Message,
            board = ConvertToList(GameBrain.board),
            win = GameBrain.win,
            playerNumber = GameBrain.playerNumber,
            player1Options = GameBrain.player1Options,
            player2Options = GameBrain.player2Options,
            gridX = GameBrain.gridX,
            gridY = GameBrain.gridY
        });
    }

    public class MoveChipRequest
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public string SessionId { get; set; }
    }

    public IActionResult OnPostMoveChip([FromBody] MoveChipRequest request)
    {
        SessionId = request.SessionId;
        var (config, state) = _sessionRepository.GetGameState(SessionId);
        GameBrain = new Brain(config, state);
        
        bool madeMove = GameBrain.moveChip(request.StartX, request.StartY, request.EndX, request.EndY);
        if (madeMove)
        {
            GameBrain.SaveGame(SessionId);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
        else
        {
            Message = "You can't move there";
        }
        
        GameBrain.SaveGame(SessionId);

        return new JsonResult(new
        {
            Message,
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
        public string SessionId { get; set; }
    }
    
    public IActionResult OnPostMoveBoard([FromBody] MoveBoardRequest request)
    {
        SessionId = request.SessionId;
        var (config, state) = _sessionRepository.GetGameState(SessionId);
        GameBrain = new Brain(config, state);
        
        bool madeMove = GameBrain.moveMovableBoard(request.Direction);
        if (madeMove)
        {
            GameBrain.SaveGame(SessionId);
            Message = $"Player {GameBrain.playerNumber} is thinking";
        }
        else
        {
            Message = "You can't move there";
        }

        return new JsonResult(new
        {
            Message,
            board = ConvertToList(GameBrain.board),
            win = GameBrain.win,
            playerNumber = GameBrain.playerNumber,
            player1Options = GameBrain.player1Options,
            player2Options = GameBrain.player2Options,
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
    
    public IActionResult OnPostSaveName(string sessionId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {   
            return RedirectToPage("/NewGame");
        }
        
        _sessionRepository.SaveSessionName(sessionId, name);
        
        return RedirectToPage("/NewGame");
    }
}




