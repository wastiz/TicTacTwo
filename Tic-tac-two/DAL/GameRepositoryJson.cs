using System.Text.Json;
namespace DAL;

public class GameRepositoryJson
{
    public List<GameState> _gameStates;

    public GameRepositoryJson()
    {
        _gameStates = new List<GameState>();
    }
    public void SaveGameToRepo(GameState gameState)
    {
        
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            WriteIndented = true
        };

        var configJsonStr = System.Text.Json.JsonSerializer.Serialize(gameState, options);
        System.IO.File.WriteAllText(FileHelper.BasePath + gameState.StateName + FileHelper.GameExtension, configJsonStr);
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

    public List<string> GetAllStateNames()
    {
        List<string> gameNames = new List<string>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var gameJsonStr = System.IO.File.ReadAllText(file);
            var game = System.Text.Json.JsonSerializer.Deserialize<GameState>(gameJsonStr);
            
            if (game != null)
            {
                gameNames.Add(game.StateName);
            }
        }
        return gameNames;
    }

    public GameState GetGameStateByName(string name)
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