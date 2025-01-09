using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SessionRepository
{
    private readonly AppDbContext _context;
    
    public SessionRepository()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
    }

    public GameSession CreateGameSession(string? configId = null, string? player1Id = null, string? gameMode = null, string? password = null)
    {
        var gameSession = new GameSession()
        {
            GameConfigId = configId ?? null,
            Player1Id = player1Id ?? null,
            GameMode = gameMode ?? null,
            GamePassword = password ?? GenerateNumericPassword(6)
        };
        
        _context.GameSessions.Add(gameSession);
        _context.SaveChanges();
        
        return gameSession;
    }
    
    static string GenerateNumericPassword(int length)
    {
        Random random = new Random();
        string password = "";

        for (int i = 0; i < length; i++)
        {
            password += random.Next(0, 10);
        }

        return password;
    }

    public GameSession GetSessionById(string sessionId)
    {
        var existingSession = _context.GameSessions.SingleOrDefault(session => session.Id == sessionId);
        if (existingSession == null)
        {
            throw new KeyNotFoundException($"Game Session with ID '{sessionId}' not found.");
        }
        return existingSession;
    }

    public List<GameSessionDto> GetSessionDtos()
    {
        return _context.GameSessions.Select(session => new GameSessionDto()
            {
                SessionId = session.Id,
                SessionName = session.Name,
            })
            .ToList();
    }

    public void SaveGameState(GameState gameState, string sessionId)
    {
        var existingSession = _context.GameSessions.SingleOrDefault(session => session.Id == sessionId);
        if (existingSession == null)
        {
            throw new KeyNotFoundException($"Game Session with ID '{sessionId}' not found.");
        }

        existingSession.GameState = gameState;
        _context.SaveChanges();
    }

    public void SaveSessionName(string sessionName, string sessionId)
    {
        var existingSession = _context.GameSessions.SingleOrDefault(session => session.Id == sessionId);
        if (existingSession == null)
        {
            throw new KeyNotFoundException($"Game Session with ID '{sessionId}' not found.");
        }
        existingSession.Name = sessionName;
        _context.SaveChanges();
    }

}
