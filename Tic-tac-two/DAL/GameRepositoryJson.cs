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
        // var configJsonStr = System.Text.Json.JsonSerializer.Serialize(gameState);
        // System.IO.File.WriteAllText(FileHelper.BasePath + gameState.StateName + FileHelper.ConfigExtension, configJsonStr);
        
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            WriteIndented = true
        };

        var configJsonStr = System.Text.Json.JsonSerializer.Serialize(gameState, options);
        System.IO.File.WriteAllText(FileHelper.BasePath + gameState.StateName + FileHelper.ConfigExtension, configJsonStr);
    }

}