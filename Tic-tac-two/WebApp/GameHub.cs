using DAL;
using GameBrain;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace WebApp.Hubs
{
    public class GameHub : Hub
    {
        // Concurrent dictionary для хранения состояний игр
        private static readonly ConcurrentDictionary<string, GameState> Games = new();

        public async Task JoinGame(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

            // Если игра не существует, создаем новое состояние игры
            Games.TryAdd(sessionId, new GameState(sessionId));

            // Уведомляем всех участников группы о подключении игрока
            await Clients.Group(sessionId).SendAsync("PlayerJoined", Context.ConnectionId);
        }

        public async Task MakeMove(string sessionId, string userId, int x, int y)
        {
            if (!Games.TryGetValue(sessionId, out var gameState))
            {
                await Clients.Caller.SendAsync("Error", "Game not found.");
                return;
            }

            var response = gameState.MakeMove(userId, x, y);
            if (!response.Success)
            {
                await Clients.Caller.SendAsync("Error", response.Message);
                return;
            }

            // Рассылка обновленного состояния игры всем участникам группы
            await Clients.Group(sessionId).SendAsync("GameStateUpdated", response.GameState);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            // Здесь можно обработать отключение игроков, если это требуется
            await base.OnDisconnectedAsync(exception);
        }
    }

    public class GameState
    {
        public string SessionId { get; }
        public Brain GameBrain { get; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public bool IsPlayerTurn(string userId)
        {
            return (userId == Player1Id && GameBrain.playerNumber == 1) || 
                   (userId == Player2Id && GameBrain.playerNumber == 2);
        }

        public GameState(string sessionId)
        {
            SessionId = sessionId;
            GameBrain = new Brain(); // Замените на вашу логику инициализации игры
        }

        public (bool Success, string Message, GameState GameState) MakeMove(string userId, int x, int y)
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

            return (true, "Move successful.", this);
        }
    }
}
