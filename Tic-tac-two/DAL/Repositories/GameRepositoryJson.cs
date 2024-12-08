using System.Text.Json;
using DAL.DTO;

namespace DAL;

public class GameRepositoryJson : IStateRepository
{
    public List<GameState> _gameStates;

    public GameRepositoryJson()
    {
        _gameStates = new List<GameState>();
    }
    public void SaveGameState(GameState gameState)
    {
        
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            WriteIndented = true
        };

        var configJsonStr = System.Text.Json.JsonSerializer.Serialize(gameState, options);
        System.IO.File.WriteAllText(FileHelper.BasePath + gameState.Name + FileHelper.GameExtension, configJsonStr);
    }

    public void DeleteGameState(string stateName)
    {
        string filePath = FileHelper.BasePath + stateName + FileHelper.GameExtension;

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
            Console.WriteLine($"Game state {stateName} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Game state {stateName} not found.");
        }
    }

    public List<GameState> GetAllGameStates()
    {
        List<GameState> gameStates = new List<GameState>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var gameJsonStr = System.IO.File.ReadAllText(file);
            var game = System.Text.Json.JsonSerializer.Deserialize<GameState>(gameJsonStr);
            
            if (game != null)
            {
                gameStates.Add(game);
            }
        }

        return gameStates;
    }

    public List<GameStateDto> GetAllStateDto()
    {
        List<GameStateDto> gameNames = new List<GameStateDto>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var gameJsonStr = System.IO.File.ReadAllText(file);
            var game = System.Text.Json.JsonSerializer.Deserialize<GameState>(gameJsonStr);
            
            if (game != null)
            {
                gameNames.Add(new GameStateDto()
                {
                    Id = game.Id,
                    Name = game.Name
                });
            }
        }
        return gameNames;
    }

    public GameState GetGameStateById(string name)
    {
        var statePath = FileHelper.BasePath + name + FileHelper.GameExtension;
        
        if (System.IO.File.Exists(statePath))
        {
            var stateJsonStr = System.IO.File.ReadAllText(statePath);
            var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(stateJsonStr);
            return gameState;
        }
        
        throw new FileNotFoundException($"Game State '{name}' not found.");

    }

}