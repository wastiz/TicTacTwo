using DAL.Contracts;
using Domain;
using Microsoft.EntityFrameworkCore;
using Shared.GameConfigDtos;
using Shared.GameSessionDtos;
using Shared.GameStateDtos;

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

    public GameSessionDto? GetSessionById(string sessionId)
    {
        var session = _context.GameSessions
            .Include(s => s.GameConfiguration)
            .Include(s => s.GameState)
            .SingleOrDefault(s => s.Id == sessionId);

        if (session == null)
            return null;

        if (session.GameState == null)
        {
            session.GameState = new GameState();
            _context.SaveChanges();
        }

        return new GameSessionDto
        {
            Id = session.Id,
            Name = session.Name,
            GameConfiguration = session.GameConfiguration != null ? new GameConfigDto
            {
                Id = session.GameConfiguration.Id,
                Name = session.GameConfiguration.Name,
                BoardSizeWidth = session.GameConfiguration.BoardSizeWidth,
                BoardSizeHeight = session.GameConfiguration.BoardSizeHeight,
                MovableBoardWidth = session.GameConfiguration.MovableBoardWidth,
                MovableBoardHeight = session.GameConfiguration.MovableBoardHeight,
                Player1Chips = session.GameConfiguration.Player1Chips,
                Player2Chips = session.GameConfiguration.Player2Chips,
                WinCondition = session.GameConfiguration.WinCondition,
                OptionsAfterNMoves = session.GameConfiguration.OptionsAfterNMoves
            } : null,
            GameState = session.GameState != null ? new GameStateDto
            {
                Id = session.GameState.Id,
                Board = session.GameState.Board,
                ChipsLeft = session.GameState.ChipsLeft,
                PlayersMoves = session.GameState.PlayersMoves,
                GridX = session.GameState.GridX,
                GridY = session.GameState.GridY,
                PlayerNumber = session.GameState.PlayerNumber,
                Player1Options = session.GameState.Player1Options,
                Player2Options = session.GameState.Player2Options,
                Win = session.GameState.Win
            } : null,
            Player1Id = session.Player1Id,
            Player1Username = session.Player1.Username,
            Player2Id = session.Player2Id,
            Player2Username = session.Player2.Username,
            GameMode = session.GameMode,
            GamePassword = session.GamePassword,
            GameStatus = session.GameStatus,
            CreatedAt = session.CreatedAt,
            LastSaveAt = session.LastSaveAt
        };
    }

    public GameSession? GetDomainSessionById(string sessionId)
    {
        return _context.GameSessions
            .Include(s => s.GameConfiguration)
            .Include(s => s.GameState)
            .SingleOrDefault(s => s.Id == sessionId);
    }
    
    public List<GameSessionDto> GetUserSessionDto(string userId)
    {
        var sessions = _context.GameSessions
            .Where(session => session.Player1Id == userId)
            .Include(s => s.GameConfiguration)
            .Include(s => s.GameState)
            .ToList();

        var result = new List<GameSessionDto>();

        foreach (var session in sessions)
        {
            if (session.GameState == null)
            {
                session.GameState = new GameState();
                _context.SaveChanges();
            }

            result.Add(new GameSessionDto
            {
                Id = session.Id,
                Name = session.Name,
                GameConfiguration = session.GameConfiguration != null ? new GameConfigDto
                {
                    Id = session.GameConfiguration.Id,
                    Name = session.GameConfiguration.Name,
                    BoardSizeWidth = session.GameConfiguration.BoardSizeWidth,
                    BoardSizeHeight = session.GameConfiguration.BoardSizeHeight,
                    MovableBoardWidth = session.GameConfiguration.MovableBoardWidth,
                    MovableBoardHeight = session.GameConfiguration.MovableBoardHeight,
                    Player1Chips = session.GameConfiguration.Player1Chips,
                    Player2Chips = session.GameConfiguration.Player2Chips,
                    WinCondition = session.GameConfiguration.WinCondition,
                    OptionsAfterNMoves = session.GameConfiguration.OptionsAfterNMoves
                } : null,
                GameState = session.GameState != null ? new GameStateDto
                {
                    Id = session.GameState.Id,
                    Board = session.GameState.Board,
                    ChipsLeft = session.GameState.ChipsLeft,
                    PlayersMoves = session.GameState.PlayersMoves,
                    GridX = session.GameState.GridX,
                    GridY = session.GameState.GridY,
                    PlayerNumber = session.GameState.PlayerNumber,
                    Player1Options = session.GameState.Player1Options,
                    Player2Options = session.GameState.Player2Options,
                    Win = session.GameState.Win
                } : null,
                Player1Id = session.Player1Id,
                Player1Username = session.Player1.Username,
                Player2Id = session.Player2Id,
                Player2Username = session.Player2.Username,
                GameMode = session.GameMode,
                GamePassword = session.GamePassword,
                GameStatus = session.GameStatus,
                CreatedAt = session.CreatedAt,
                LastSaveAt = session.LastSaveAt
            });
        }

        return result;
    }
    
    public (GameConfigDto config, GameStateDto state) GetGameState(string sessionId)
    {
        var session = GetSessionById(sessionId);
        return (session.GameConfiguration, session.GameState);
    }

    public void SaveSecondPlayer(string sessionId, string player2Id)
    {
        var session = GetDomainSessionById(sessionId);
        session.Player2Id = player2Id;
        _context.SaveChanges();
    }

    public GameState SaveGameState(GameState gameState, string sessionId)
    {
        var existingSession = GetDomainSessionById(sessionId);
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
