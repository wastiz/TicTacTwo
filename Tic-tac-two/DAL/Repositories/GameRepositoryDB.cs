using System.Collections.Generic;
using System.Linq;
using DAL;
using Microsoft.EntityFrameworkCore;

public class GameRepositoryDb
{
    private readonly GameStateDbContext _context;

    public GameRepositoryDb()
    {
        _context = new GameStateDbContext();
        _context.Database.EnsureCreated();
    }
    
    private GameStateDB ConvertToDbModel(GameState gameState)
    {
        return new GameStateDB
        {
            StateName = gameState.StateName,
            GameConfig = gameState.GameConfig,
            Board = gameState.Board,
            GridX = gameState.GridX,
            GridY = gameState.GridY,
            ChipsLeft = gameState.ChipsLeft,
            PlayerNumber = gameState.PlayerNumber
        };
    }
    
    private GameState ConvertFromDbModel(GameStateDB gameStateDb)
    {
        return new GameState
        {
            StateName = gameStateDb.StateName,
            GameConfig = gameStateDb.GameConfig,
            Board = gameStateDb.Board,
            GridX = gameStateDb.GridX,
            GridY = gameStateDb.GridY,
            ChipsLeft = gameStateDb.ChipsLeft,
            PlayerNumber = gameStateDb.PlayerNumber
        };
    }

    public void SaveGameToRepo(GameState gameState)
    {
        var gameStateDb = ConvertToDbModel(gameState);
        var existingState = _context.GameStates.SingleOrDefault(gs => gs.StateName == gameState.StateName);
        if (existingState == null)
        {
            _context.GameStates.Add(gameStateDb);
        }
        else
        {
            _context.Entry(existingState).CurrentValues.SetValues(gameStateDb);
        }

        _context.SaveChanges();
    }

    public List<GameState> GetAllGameStates()
    {
        return _context.GameStates
            .Select(gsDb => ConvertFromDbModel(gsDb))
            .ToList();
    }

    public List<string> GetAllStateNames()
    {
        return _context.GameStates.Select(gs => gs.StateName).ToList();
    }

    public GameState GetGameStateByName(string name)
    {
        var gameStateDb = _context.GameStates.SingleOrDefault(gs => gs.StateName == name);

        if (gameStateDb == null)
        {
            throw new KeyNotFoundException($"Game State '{name}' not found.");
        }

        return ConvertFromDbModel(gameStateDb);
    }

    public void DeleteGameState(string name)
    {
        var gameState = _context.GameStates.SingleOrDefault(gs => gs.StateName == name);
        if (gameState == null)
        {
            throw new KeyNotFoundException($"Game State '{name}' not found.");
        }

        _context.GameStates.Remove(gameState);
        _context.SaveChanges();
    }
}
