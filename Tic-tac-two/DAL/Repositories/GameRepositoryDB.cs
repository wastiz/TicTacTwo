using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DAL;
using Microsoft.EntityFrameworkCore;

public class GameRepositoryDb : IStateRepository
{
    private readonly AppDbContext _context;

    public GameRepositoryDb()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
    }
    
    private GameStateDB ConvertToDbModel(GameState gameState)
    {
        return new GameStateDB
        {
            Name = gameState.StateName,
            GameConfig = JsonSerializer.Serialize(gameState.GameConfig),
            Board = JsonSerializer.Serialize(gameState.Board),
            ChipsLeft = JsonSerializer.Serialize(gameState.ChipsLeft),
            PlayersMoves = JsonSerializer.Serialize(gameState.PlayersMoves),
            GridX = gameState.GridX,
            GridY = gameState.GridY,
            PlayerNumber = gameState.PlayerNumber,
            Player1Options = gameState.Player1Options,
            Player2Options = gameState.Player2Options,
            Win = gameState.Win,
        };
    }
    
    private GameState ConvertFromDbModel(GameStateDB gameStateDb)
    {
        var config = JsonSerializer.Deserialize<GameConfiguration>(gameStateDb.GameConfig);
        
        return new GameState
        {
            StateName = gameStateDb.Name,
            GameConfig = config != null ? config : new GameConfiguration(),
            Board = JsonSerializer.Deserialize<int[][]>(gameStateDb.Board) ?? Array.Empty<int[]>(),
            ChipsLeft = JsonSerializer.Deserialize<int[]>(gameStateDb.ChipsLeft) ?? Array.Empty<int>(),
            PlayersMoves = JsonSerializer.Deserialize<int[]>(gameStateDb.PlayersMoves) ?? Array.Empty<int>(),
            GridX = gameStateDb.GridX,
            GridY = gameStateDb.GridY,
            PlayerNumber = gameStateDb.PlayerNumber,
            Player1Options = gameStateDb.Player1Options,
            Player2Options = gameStateDb.Player2Options,
            Win = gameStateDb.Win,
        };
    }

    public void SaveGameState(GameState gameState)
    {
        var gameStateDb = ConvertToDbModel(gameState);
        var existingState = _context.GameStates.SingleOrDefault(gs => gs.Name == gameState.StateName);

        if (existingState == null)
        {
            _context.GameStates.Add(gameStateDb);
        }
        else
        {
            existingState.GameConfig = gameStateDb.GameConfig;
            existingState.Board = gameStateDb.Board;
            existingState.ChipsLeft = gameStateDb.ChipsLeft;
            existingState.PlayersMoves = gameStateDb.PlayersMoves;
            existingState.GridX = gameStateDb.GridX;
            existingState.GridY = gameStateDb.GridY;
            existingState.PlayerNumber = gameStateDb.PlayerNumber;
            existingState.Player1Options = gameStateDb.Player1Options;
            existingState.Player2Options = gameStateDb.Player2Options;
            existingState.Win = gameStateDb.Win;
            
            _context.Entry(existingState).State = EntityState.Modified;
            _context.Entry(existingState).Property(e => e.Id).IsModified = false;
        }

        _context.SaveChanges();
    }
    
    public void DeleteGameState(string name)
    {
        var gameState = _context.GameStates.SingleOrDefault(gs => gs.Name == name);
        if (gameState == null)
        {
            throw new KeyNotFoundException($"Game State '{name}' not found.");
        }

        _context.GameStates.Remove(gameState);
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
        return _context.GameStates.Select(gs => gs.Name).ToList();
    }

    public GameState GetGameStateByName(string name)
    {
        var gameStateDb = _context.GameStates.SingleOrDefault(gs => gs.Name == name);

        if (gameStateDb == null)
        {
            throw new KeyNotFoundException($"Game State '{name}' not found.");
        }

        return ConvertFromDbModel(gameStateDb);
    }

    public string GetGameIdByName(string name)
    {
        return "some str";
    }
}
