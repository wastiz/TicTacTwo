using DAL;
using GameBrain;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace WebApp.Hubs
{ 
    public class GameHub : Hub
    {
        private static readonly ConcurrentDictionary<string, GameState> Games = new();
        private readonly GameRepositoryDb _gameRepositoryDb;
        private readonly AppDbContext _context;
        public GameSessionDB Session;
        public string StateId;
        public Brain GameBrain { get; set; }

        public GameHub(AppDbContext context, GameRepositoryDb gameRepositoryDb)
        {
            _gameRepositoryDb = gameRepositoryDb;
            _context = context;
        }

        public async Task JoinGame(string sessionId, string userId)
        {
            if (!Games.ContainsKey(sessionId))
            {
                Session = _context.GameSessions.FirstOrDefault(s => s.Id == sessionId);
                StateId = Session.GameStateId;
                GameBrain = new Brain(_gameRepositoryDb.GetGameStateById(StateId));
                Games.TryAdd(sessionId, new GameState(sessionId, GameBrain));
            }

            var gameState = Games[sessionId];

            if ((gameState.Player1Id == userId || gameState.Player2Id == userId) && (gameState.Player1Id != null && gameState.Player2Id != null))
            {
                await Clients.Group(sessionId).SendAsync("GameStarted", gameState.GetGameState(userId));
                return;
            }
            
            if (gameState.Player1Id != null && gameState.Player2Id != null)
            {
                await Clients.Caller.SendAsync("Error", "Game is already full.");
                return;
            }

            if (gameState.Player1Id == null)
            {
                gameState.Player1Id = userId;
                gameState.Player1Name = _context.Users.FirstOrDefault(u => u.Id == userId).Username;
                gameState.SetCurrentPlayerName(userId);
                gameState.Message = $"Player {gameState.CurrentPlayerName} is thinking.";
            }
            else if (gameState.Player2Id == null)
            {
                gameState.Player2Id = userId;
                gameState.Player2Name = _context.Users.FirstOrDefault(u => u.Id == userId).Username;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            
            await Clients.Group(sessionId).SendAsync("PlayerJoined", userId);
            
            if (gameState.Player1Id != null && gameState.Player2Id != null)
            {
                await Clients.Group(sessionId).SendAsync("GameStarted", gameState.GetGameState(userId));
            }
        }

        public async Task PlaceChip(string sessionId, string userId, int x, int y)
        {
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.PlaceChip(userId, x, y);
            if (!response.Success)
            {
                await Clients.Caller.SendAsync("Error", response.Message);
                return;
            }
            
            await Clients.Group(sessionId).SendAsync("GameStateUpdated", response.GameState);
        }


        public async Task MoveChip(string sessionId, string userId, int startX, int startY, int endX, int endY)
        {
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.MoveChip(userId, startX, startY, endX, endY);
            if (!response.Success)
            {
                await Clients.Caller.SendAsync("Error", response.Message);
                return;
            }

            await Clients.Group(sessionId).SendAsync("GameStateUpdated", response.GameState);
        }
        
        public async Task MoveBoard(string sessionId, string userId, string direction)
        {
            Console.WriteLine($"MoveBoard: userId={userId}, direction={direction}");
            
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.MoveBoard(userId, direction);
            if (!response.Success)
            {
                await Clients.Caller.SendAsync("Error", response.Message);
                return;
            }

            await Clients.Group(sessionId).SendAsync("GameStateUpdated", response.GameState);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var sessionId in Games.Keys)
            {
                if (Games.TryGetValue(sessionId, out var gameState))
                {
                    if (gameState.Player1Id == Context.ConnectionId || gameState.Player2Id == Context.ConnectionId)
                    {
                        Games.TryRemove(sessionId, out _);
                        await Clients.Group(sessionId).SendAsync("GameEnded", "A player has disconnected.");
                        break;
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }


    public class GameState
    {
        public string SessionId { get; }
        public Brain GameBrain { get; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string CurrentPlayerName { get; set; }
        public string Message {get; set;}
        public bool IsPlayerTurn(string userId)
        {
            return (userId == Player1Id && GameBrain.playerNumber == 1) || 
                   (userId == Player2Id && GameBrain.playerNumber == 2);
        }

        public void SetCurrentPlayerName(string userId)
        {
            if (GameBrain.playerNumber == 1 && userId == Player1Id)
            {
                CurrentPlayerName = Player2Name;
            } else if (GameBrain.playerNumber == 2 && userId == Player2Id)
            {
                CurrentPlayerName = Player1Name;
            }
        }

        public GameState(string sessionId, Brain gameBrain)
        {
            SessionId = sessionId;
            GameBrain = gameBrain;
        }

        public (bool Success, string Message, object GameState) PlaceChip(string userId, int x, int y)
        {
            if (!IsPlayerTurn(userId))
            {
                return (false, "It's not your turn!", null);
            }

            bool moveSuccess = GameBrain.placeChip(x, y);
            if (!moveSuccess)
            {
                return (false, "Invalid move.", null);
            }
            SetCurrentPlayerName(userId);
            Message = $"Player {CurrentPlayerName} is thinking.";
            return (true, "Move successful.", GetGameState(userId));
        }
        
        public (bool Success, string Message, object GameState) MoveChip (string userId, int startX, int startY, int endX, int endY)
        {
            if (!IsPlayerTurn(userId))
            {
                return (false, "It's not your turn!", null);
            }

            bool moveSuccess = GameBrain.moveChip(startX, startY, endX, endY);
            if (!moveSuccess)
            {
                return (false, "Invalid move.", null);
            }
            SetCurrentPlayerName(userId);
            Message = $"Player {CurrentPlayerName} is thinking.";

            return (true, "Move successful.", GetGameState(userId));
        }
        
        public (bool Success, string Message, object GameState) MoveBoard (string userId, string direction)
        {
            if (!IsPlayerTurn(userId))
            {
                return (false, "It's not your turn!", null);
            }

            bool moveSuccess = GameBrain.moveMovableBoard(direction);
            if (!moveSuccess)
            {
                return (false, "Invalid move.", null);
            }
            SetCurrentPlayerName(userId);
            Message = $"Player {CurrentPlayerName} is thinking.";

            return (true, "Move successful.", GetGameState(userId));
        }


        public object GetGameState(string userId)
        {
            return new
            {
                board = ConvertToList(GameBrain.board),
                IsPlayerTurn = IsPlayerTurn(userId),
                gridX = GameBrain.gridX,
                gridY = GameBrain.gridY,
                player1Options = GameBrain.player1Options,
                player2Options = GameBrain.player2Options,
                win = GameBrain.win,
                message = Message,
            };
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
