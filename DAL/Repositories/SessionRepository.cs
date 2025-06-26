using DAL.Contracts;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;
    
    public SessionRepository()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
    }

    public GameSession CreateGameSession(string configId, string gameMode, string player1Id, string? password = null)
    {
        var config = _context.GameConfigurations.FirstOrDefault(c => c.Id == configId);
        if (config == null)
        {
            throw new ArgumentException($"Game configuration with ID {configId} not found.");
        }

        var initialState = CreateInitialGameState(config);

        var gameSession = new GameSession
        {
            GameConfigId = configId,
            GameStateId = initialState.Id,
            Player1Id = player1Id,
            GameMode = gameMode,
            GamePassword = password ?? GenerateNumericPassword(6),
        };

        _context.GameSessions.Add(gameSession);
        _context.SaveChanges();

        return gameSession;
    }

    
    public GameState CreateInitialGameState(GameConfiguration config)
    {
        int height = config.BoardSizeHeight;
        int width = config.BoardSizeWidth;

        int[][] board = new int[height][];
        for (int i = 0; i < height; i++)
        {
          board[i] = new int[width];
        }
        
        var initialState = new GameState
        {
            Board = board,
            ChipsLeft = new int[] { 0, config.Player1Chips, config.Player2Chips },
            PlayersMoves = new int[] { 0, 0, 0 },
            GridX = (config.BoardSizeWidth - config.MovableBoardWidth) / 2,
            GridY = (config.BoardSizeHeight - config.MovableBoardHeight) / 2,
            PlayerNumber = 1,
            Player1Options = false,
            Player2Options = false,
            Win = 0
        };
        
        _context.GameStates.Add(initialState);
        _context.SaveChanges();

        return initialState;
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

    public GameSession? GetSessionById(string sessionId)
    {
        var session = _context.GameSessions
            .Include(s => s.GameConfiguration)
            .Include(s => s.GameState)
            .SingleOrDefault(s => s.Id == sessionId);

        if (session != null && session.GameState == null)
        {
            session.GameState = new GameState();
            _context.SaveChanges();
        }

        return session;
    }


    public List<GameSession> GetUserSessionDto(string userId)
    {
        return _context.GameSessions
            .Where(session => session.Player1Id == userId)
            .ToList();
    }


    public (GameConfiguration config, GameState state) GetGameState(string sessionId)
    {
        var session = GetSessionById(sessionId);
        return (session.GameConfiguration, session.GameState);
    }

    public void SaveSecondPlayer(GameSession session, string player2Id)
    {
        session.Player2Id = player2Id;
        _context.SaveChanges();
    }

    public GameState SaveGameState(GameState gameState, string sessionId)
    {
        var existingSession = GetSessionById(sessionId);
        if (existingSession == null)
        {
            throw new KeyNotFoundException($"Game Session with ID '{sessionId}' not found.");
        }

        existingSession.GameState = gameState;
        _context.SaveChanges();

        return existingSession.GameState;
    }


    public void SaveSessionName(string sessionId, string sessionName)
    {
        var existingSession = GetSessionById(sessionId);
        if (existingSession == null)
        {
            throw new KeyNotFoundException($"Game Session with ID '{sessionId}' not found.");
        }
        existingSession.Name = sessionName;
        _context.SaveChanges();
    }


    public void DeleteSession(string sessionId)
    {
        var session = _context.GameSessions.SingleOrDefault(s => s.Id == sessionId);
            
        if (session == null)
        {
            throw new KeyNotFoundException($"Session '{sessionId}' not found.");
        }

        _context.GameSessions.Remove(session);
        _context.SaveChanges();
    }
}
