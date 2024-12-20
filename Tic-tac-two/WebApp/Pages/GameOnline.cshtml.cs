﻿using DAL;
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
        public string UserId;
        public string Username;
        public Brain GameBrain { get; set; }
        private readonly GameRepositoryDb _gameRepositoryDb;
        private AppDbContext _context;
        [BindProperty] public string Message { get; set; }

        public GameOnline(AppDbContext context, GameRepositoryDb gameRepositoryDb)
        {
            _gameRepositoryDb = gameRepositoryDb;
            _context = context;
        }

        public void OnGet()
        {
            Session = _context.GameSessions.FirstOrDefault(s => s.Id == SessionId);
    
            if (Session == null)
            {
                Message = "Game session not found!";
                return;
            }
            
            StateId = Session.GameStateId;
            var gameState = _gameRepositoryDb.GetGameStateById(StateId);
            
            if (gameState == null)
            {
                Message = "Game state not found!";
                return;
            }

            UserId = HttpContext.Session.GetString("UserId");
            Username = HttpContext.Session.GetString("Username");
            GameBrain = new Brain(gameState);
            
            if (UserId == Session.Player1Id)
            {
                ViewData["PlayerRole"] = 1;
            }
            else if (UserId == Session.Player2Id)
            {
                ViewData["PlayerRole"] = 2;
            }
            else
            {
                ViewData["PlayerRole"] = 0;
            }

            GameBrain = new Brain(gameState);
        }
        
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
                currentPlayer = GameBrain.playerNumber == 1 ? Session.Player1Id : Session.Player2Id,
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
            public string GameId { get; set; }
        }

        public IActionResult OnPostMoveChip([FromBody] MoveChipRequest request)
        {
            var gameState = _gameRepositoryDb.GetGameStateById(request.GameId);
            GameBrain = new Brain(gameState);
            
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
        
        public IActionResult OnPostGetState([FromBody] StateRequest request)
        {
            var gameState = _gameRepositoryDb.GetGameStateById(request.GameId);
            GameBrain = new Brain(gameState);

            return new JsonResult(new
            {
                board = ConvertToList(GameBrain.board),
                win = GameBrain.win,
                playerNumber = GameBrain.playerNumber,
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY
            });
        }
        
        public class StateRequest
        {
            public string GameId { get; set; }
        }
    }
}