﻿using DAL.DTO;
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

    public GameSession CreateGameSession(GameConfiguration config, string? player1Id = null, string? gameMode = null, string? password = null)
    {
        if (config == null)
        {
            throw new ArgumentException($"Game configuration not found.");
        }
        
        var initialState = CreateInitialGameState(config);
        
        var gameSession = new GameSession
        {
            GameConfigId = config.Id,
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
        var initialState = new GameState
        {
            Board = new int[config.BoardSizeHeight, config.BoardSizeWidth],
            ChipsLeft = new int[] { 0, config.ChipsCount[1], config.ChipsCount[2] },
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
