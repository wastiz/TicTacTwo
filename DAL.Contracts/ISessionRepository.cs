using Domain;
using Shared.GameConfigDtos;
using Shared.GameSessionDtos;
using Shared.GameStateDtos;

namespace DAL.Contracts
{
    public interface ISessionRepository
    {
        GameSession CreateGameSession(string configId, string gameMode, string player1Id, string? password = null);
        GameState CreateInitialGameState(GameConfiguration config);
        GameSessionDto? GetSessionById(string sessionId);
        GameSession? GetDomainSessionById(string sessionId);
        List<GameSessionDto> GetUserSessionDto(string userId);
        (GameConfigDto config, GameStateDto state) GetGameState(string sessionId);
        void SaveSecondPlayer(string sessionId, string player2Id);
        GameState SaveGameState(GameState gameState, string sessionId);
        void SaveSessionName(string sessionId, string sessionName);
        void DeleteSession(string sessionId);
    }
}
