namespace DAL;

public class ConfigRepositoryJson : ConfigRepository
{
    public ConfigRepositoryJson()
    {
        
    }
    public string GetGameConfiguration()
    { 
        var result = File.ReadAllText("options.json");
        return result;
    }

    public void WriteGameConfiguration(List<string> gameConfiguration)
    {
        foreach (var optionValue in gameConfiguration)
        {
            var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(optionValue);
            File.WriteAllText("options.json",optionJsonStr);
        }
    }
}