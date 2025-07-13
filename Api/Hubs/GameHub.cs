using DAL;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using DAL.Contracts;
using DAL.Contracts.Interfaces;
using Domain;
using Shared;
using Shared.GameDtos;
using Shared.GameStateDtos;

namespace Api.Hubs
{ 
    public class GameHub : Hub
    {
        private static readonly ConcurrentDictionary<string, GameState> Games = new();
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        public GameSession Session;
        public GameBrain GameBrain { get; set; }

        public GameHub(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public async Task JoinGame(string sessionId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                await Clients.Caller.SendAsync("Error", "User ID is missing.");
                return;
            }

            if (!Games.ContainsKey(sessionId))
            {
                Session = _sessionRepository.GetDomainSessionById(sessionId);
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
                await Clients.Caller.SendAsync("GameStateUpdated", gameState.GetGameState(userId));
                return;
            }
            
            if (gameState.Player1Id == null)
            {
                gameState.Player1Id = userId;
                gameState.Player1Name = await _userRepository.GetUsernameById(userId);
            }
            else if (gameState.Player2Id == null)
            {
                gameState.Player2Id = userId;
                gameState.Player2Name = await _userRepository.GetUsernameById(userId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Game already has two players.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            await Clients.Group(sessionId).SendAsync("PlayerJoined", userId);

            if (gameState.Player1Id != null && gameState.Player2Id != null)
            {
                await Clients.Group(sessionId).SendAsync("GameStarted", gameState.GetGameState(userId));
            }
        }
        
        public async Task PlaceChip(string sessionId, string userId, PlaceChipRequest request)
        {
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.PlaceChip(userId, request.X, request.Y);
            if (!response.Success)
            {
                await Clients.Caller.SendAsync("Error", response.Message);
                return;
            }
    
            await Clients.Group(sessionId).SendAsync("GameStateUpdated", response.GameState);
        }


        public async Task MoveChip(string sessionId, string userId, MoveChipRequest request)
        {
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.MoveChip(userId, request.StartX, request.StartY, request.EndX, request.EndY);
            if (!response.Success)
            {
                await Clients.Caller.SendAsync("Error", response.Message);
                return;
            }

            await Clients.Group(sessionId).SendAsync("GameStateUpdated", response.GameState);
        }
        
        public async Task MoveBoard(string sessionId, string userId, MoveBoardRequest request)
        {
            Console.WriteLine($"MoveBoard: userId={userId}, direction={request.Direction}");
            
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.MoveBoard(userId, request.Direction);
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
            if (Player1Id == userId && GameBrain.PlayerNumber == 1) return true;
            if (Player2Id == userId && GameBrain.PlayerNumber == 2) return true;
            return false;
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

            Response response = GameBrain.PlaceChip(x, y);
            if (!response.Success)
            {
                return (response.Success, response.Message, null);
            }
            
            return (true, "Move successful.", GetGameState(userId));
        }

        public (bool Success, string Message, object GameState) MoveChip(string userId, int startX, int startY, int endX, int endY)
        {
            if (!IsPlayerTurn(userId))
            {
                return (false, "It's not your turn!", null);
            }

            Response response = GameBrain.MoveChip(startX, startY, endX, endY);
            if (!response.Success)
            {
                return (response.Success, response.Message, null);
            }
            
            return (true, "Move successful.", GetGameState(userId));
        }

        public (bool Success, string Message, object GameState) MoveBoard(string userId, string direction)
        {
            if (!IsPlayerTurn(userId))
            {
                return (false, "It's not your turn!", null);
            }

            Response response = GameBrain.MoveMovableBoard(direction);
            if (!response.Success)
            {
                return (response.Success, response.Message, null);
            }
            
            
            return (true, "Move successful.", GetGameState(userId));
        }
    
        private void UpdateMessage()
        {
            if (GameBrain.PlayerNumber == 1)
            {
                Message = $"Player {Player1Name} is thinking.";
            }
            else if (GameBrain.PlayerNumber == 2)
            {
                Message = $"Player {Player2Name} is thinking.";
            }
        }


        public GameStateDto GetGameState(string userId)
        {
            UpdateMessage();
            return new GameStateDto()
            {
                Board = ConvertToList(GameBrain.Board),
                //IsPlayerTurn = IsPlayerTurn(userId),
                PlayerNumber = GameBrain.PlayerNumber,
                GridX = GameBrain.GridX,
                GridY = GameBrain.GridY,
                Player1Abilities = GameBrain.Player1Abilities,
                Player2Abilities = GameBrain.Player2Abilities,
                Win = GameBrain.Win,
                Message = Message,
            };
        }

        
        public int[][] ConvertToList(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
    
            int[][] result = new int[rows][];
    
            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = matrix[i, j];
                }
            }
    
            return result;
        }
    }
}
