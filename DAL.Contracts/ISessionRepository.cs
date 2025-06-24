using DAL.DTO;
using System.Collections.Generic;
using Shared.GameSessionDtos;

namespace DAL.Contracts
{
    public interface ISessionRepository
    {
        GameSession CreateGameSession(GameConfiguration config, string? player1Id = null, string? gameMode = null, string? password = null);
        GameState CreateInitialGameState(GameConfiguration config);
        GameSession? GetSessionById(string sessionId);
        List<GameSessionDto> GetUserSessionDto(string userId);
        (GameConfiguration config, GameState state) GetGameState(string sessionId);
        void SaveSecondPlayer(GameSession session, string player2Id);
        void SaveGameState(GameState gameState, string sessionId);
        void SaveSessionName(string sessionId, string sessionName);
        void DeleteSession(string sessionId);
    }
}