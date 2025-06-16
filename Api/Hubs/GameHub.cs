using DAL;
using GameBrain;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace WebApp.Hubs
{ 
    public class GameHub : Hub
    {
        private static readonly ConcurrentDictionary<string, GameState> Games = new();
        private readonly SessionRepository _sessionRepository;
        private readonly UserRepository _userRepository;
        public GameSession Session;
        public GameBrain.GameBrain GameBrain { get; set; }

        public GameHub(UserRepository userRepository, SessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public async Task JoinGame(string sessionId, string userId)
        {
            if (!Games.ContainsKey(sessionId))
            {
                Session = _sessionRepository.GetSessionById(sessionId);
                if (Session == null)
                {
                    await Clients.Caller.SendAsync("Error", "Game session not found.");
                    return;
                }
                
                GameBrain = new GameBrain(Session.GameConfiguration, Session.GameState);
                Games.TryAdd(sessionId, new GameState(sessionId, GameBrain));
            }

            var gameState = Games[sessionId];

            if (gameState.Player1Id == userId || gameState.Player2Id == userId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
                await Clients.Caller.SendAsync("GameStarted", gameState.GetGameState(userId));
                return;
            }

            if (gameState.Player1Id != null && gameState.Player2Id != null)
            {
                await Clients.Group(sessionId).SendAsync("GameStarted", gameState.GetGameState(userId));
            }


            if (gameState.Player1Id == null)
            {
                gameState.Player1Id = userId;
                gameState.Player1Name = _userRepository.GetUserNameById(userId);
            }
            else if (gameState.Player2Id == null)
            {
                gameState.Player2Id = userId;
                gameState.Player2Name = _userRepository.GetUserNameById(userId);
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
        public GameBrain GameBrain { get; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string Message {get; set;}
        public bool IsPlayerTurn(string userId)
        {
            return (userId == Player1Id && GameBrain.playerNumber == 1) || 
                   (userId == Player2Id && GameBrain.playerNumber == 2);
        }
        


        public GameState(string sessionId, GameBrain gameBrain)
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
            
            return (true, "Move successful.", GetGameState(userId));
        }

        public (bool Success, string Message, object GameState) MoveChip(string userId, int startX, int startY, int endX, int endY)
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
            
            return (true, "Move successful.", GetGameState(userId));
        }

        public (bool Success, string Message, object GameState) MoveBoard(string userId, string direction)
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
            
            
            return (true, "Move successful.", GetGameState(userId));
        }
    
        private void UpdateMessage()
        {
            if (GameBrain.playerNumber == 1)
            {
                Message = $"Player {Player1Name} is thinking.";
            }
            else if (GameBrain.playerNumber == 2)
            {
                Message = $"Player {Player2Name} is thinking.";
            }
        }


        public object GetGameState(string userId)
        {
            UpdateMessage();
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
