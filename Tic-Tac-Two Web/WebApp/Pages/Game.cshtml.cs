using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Game : PageModel
{
    [BindProperty(SupportsGet = true)] public string SessionId { get; set; }
    public GameSession Session;
    public Brain GameBrain { get; set; }
    private readonly SessionRepository _sessionRepository;
    private AppDbContext _context;
    [BindProperty] public string Message { get; set; }

    public Game(AppDbContext context, SessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
        _context = context;
    }

    public void OnGet()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        Session = _context.GameSessions.FirstOrDefault(s => s.Id == SessionId);

        if (Session == null)
        {
            Message = "Game session not found!";
            return;
        }
        
        GameBrain = new Brain(Session.GameConfiguration, Session.GameState);
    }

    
    public class PlaceChipRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    [HttpPost]
    public IActionResult OnPostClick([FromBody] PlaceChipRequest request)
    {
        var session = _sessionRepository.GetSessionById(SessionId);
        GameBrain = new Brain(session.GameConfiguration, session.GameState);

        bool madeMove = GameBrain.placeChip(request.X, request.Y);

        if (madeMove)
        {
            GameBrain.SaveGame(SessionId);
        }

        return new JsonResult(new
        {
            message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here",
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
    }

    public IActionResult OnPostMoveChip([FromBody] MoveChipRequest request)
    {
        var session = _sessionRepository.GetSessionById(SessionId);
        GameBrain = new Brain(session.GameConfiguration, session.GameState);
        
        bool madeMove = GameBrain.moveChip(request.StartX, request.StartY, request.EndX, request.EndY);
        string message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here";
        
        GameBrain.SaveGame(SessionId);

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
        var session = _sessionRepository.GetSessionById(SessionId);
        GameBrain = new Brain(session.GameConfiguration, session.GameState);
        
        string message;
        
        bool madeMove = GameBrain.moveMovableBoard(request.Direction);
        if (madeMove)
        {
            GameBrain.SaveGame(SessionId);
            message = madeMove ? $"Player {GameBrain.playerNumber} is thinking" : "You can't place here";
        }
        else
        {
            message = "You can't move there";
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
    
    public IActionResult OnPostSaveName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {   
            return RedirectToPage("/NewGame");
        }
        
        _sessionRepository.SaveSessionName(SessionId, name);
        
        return RedirectToPage("/NewGame");
    }
}




