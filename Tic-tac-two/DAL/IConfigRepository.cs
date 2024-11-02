namespace DAL;

public interface IConfigRepository
{
    List<string> GetAllConfigNames();
    List<GameConfiguration> GetAllConfigs();
    GameConfiguration GetConfigurationByName(string name);
    void SaveConfiguration(GameConfiguration gameConfiguration);
    void DeleteConfiguration(string name);
}