namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    public ConfigRepositoryJson(){
        CheckAndCreateInitialConfig();   
    }
    
    private void CheckAndCreateInitialConfig()
    {
        if (!System.IO.Directory.Exists(FileHelper.BasePath))
        {
            System.IO.Directory.CreateDirectory(FileHelper.BasePath);   
        }

        var data = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
        if (data.Count == 0)
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var optionNames = hardcodedRepo.GetConfigurationNames();
            foreach (var optionName in optionNames)
            {
                var gameOption = hardcodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(gameOption);
                System.IO.File.WriteAllText(FileHelper.BasePath + gameOption.Name + FileHelper.ConfigExtension, optionJsonStr);
            }
        }
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