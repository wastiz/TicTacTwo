namespace DAL;
public class ConfigRepositoryJson
{
    public List<GameConfiguration> _gameConfigurations;

    public ConfigRepositoryJson()
    {
        CheckAndCreateInitialConfig();
        _gameConfigurations = GetAllConfigs();
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
            List<GameConfiguration> initialConfigurations = new List<GameConfiguration>()
            {
                new GameConfiguration() { Name = "Classical" },
                new GameConfiguration() {
                    Name = "Big Game",
                    BoardSizeWidth = 10,
                    BoardSizeHeight = 10,
                    MovableBoardWidth = 5,
                    MovableBoardHeight = 5,
                    ChipsCount = new int[] { 0, 6, 6 },
                    WinCondition = 4,
                    OptionsAfterNMoves = 3,
                },
            };

            foreach (var config in initialConfigurations)
            {
                var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(config);
                System.IO.File.WriteAllText(FileHelper.BasePath + config.Name + FileHelper.ConfigExtension, optionJsonStr);
            }
        }
    }

    public List<string> GetAllConfigNames()
    {
        var configNames = new List<string>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension);

        foreach (var file in files)
        {
            var configJsonStr = System.IO.File.ReadAllText(file);
            var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);

            if (config != null)
            {
                configNames.Add(config.Name);
            }
        }

        return configNames;
    }

    public List<GameConfiguration> GetAllConfigs()
    {
        var configs = new List<GameConfiguration>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension);

        foreach (var file in files)
        {
            var configJsonStr = System.IO.File.ReadAllText(file);
            var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);

            if (config != null)
            {
                configs.Add(config);
            }
        }

        return configs;
    }

    public GameConfiguration GetConfigurationById(string id)
    {
        var configPath = FileHelper.BasePath + id + FileHelper.ConfigExtension;

        if (System.IO.File.Exists(configPath))
        {
            var configJsonStr = System.IO.File.ReadAllText(configPath);
            var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
            return config;
        }

        throw new FileNotFoundException($"Configuration '{id}' not found.");
    }
    
    public GameConfiguration? GetConfigurationByName(string name)
    {
        var configPath = FileHelper.BasePath + name + FileHelper.ConfigExtension;

        if (!System.IO.File.Exists(configPath))
        {
            return null;
        }

        try
        {
            var configJsonStr = System.IO.File.ReadAllText(configPath);
            return System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return null;
        }
    }
    
    public bool ConfigurationExists(string configName)
    {
        var configPath = FileHelper.BasePath + configName + FileHelper.ConfigExtension;
        return System.IO.File.Exists(configPath);
    }

    public bool SaveConfiguration(GameConfiguration gameConfiguration)
    {
        var configPath = FileHelper.BasePath + gameConfiguration.Name + FileHelper.ConfigExtension;
        
        if (System.IO.File.Exists(configPath))
        {
            return false;
        }
        
        var configJsonStr = System.Text.Json.JsonSerializer.Serialize(gameConfiguration);
        
        System.IO.File.WriteAllText(configPath, configJsonStr);

        return true;
    }


    public void DeleteConfiguration(string name)
    {
        var configPath = FileHelper.BasePath + name + FileHelper.ConfigExtension;

        if (System.IO.File.Exists(configPath))
        {
            System.IO.File.Delete(configPath);
        }
        else
        {
            throw new FileNotFoundException($"Configuration '{name}' not found.");
        }
    }
}
