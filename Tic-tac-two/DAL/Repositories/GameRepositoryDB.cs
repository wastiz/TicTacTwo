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
            StateName = gameState.StateName,
            GameConfigJson = JsonSerializer.Serialize(gameState.GameConfig),
            BoardJson = JsonSerializer.Serialize(gameState.Board),
            ChipsLeftJson = JsonSerializer.Serialize(gameState.ChipsLeft),
            PlayersMovesJson = JsonSerializer.Serialize(gameState.PlayersMoves),
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
        var config = JsonSerializer.Deserialize<GameConfiguration>(gameStateDb.GameConfigJson);
        
        return new GameState
        {
            StateName = gameStateDb.StateName,
            GameConfig = config != null ? config : new GameConfiguration(),
            Board = JsonSerializer.Deserialize<int[][]>(gameStateDb.BoardJson) ?? Array.Empty<int[]>(),
            ChipsLeft = JsonSerializer.Deserialize<int[]>(gameStateDb.ChipsLeftJson) ?? Array.Empty<int>(),
            PlayersMoves = JsonSerializer.Deserialize<int[]>(gameStateDb.PlayersMovesJson) ?? Array.Empty<int>(),
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
        var existingState = _context.GameStates.SingleOrDefault(gs => gs.StateName == gameState.StateName);

        if (existingState == null)
        {
            _context.GameStates.Add(gameStateDb);
        }
        else
        {
            existingState.GameConfigJson = gameStateDb.GameConfigJson;
            existingState.BoardJson = gameStateDb.BoardJson;
            existingState.ChipsLeftJson = gameStateDb.ChipsLeftJson;
            existingState.PlayersMovesJson = gameStateDb.PlayersMovesJson;
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
        var gameState = _context.GameStates.SingleOrDefault(gs => gs.StateName == name);
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
}
