namespace DAL;

public interface IConfigRepository
{
    List<string> GetAllConfigNames();
    List<GameConfigurationDB> GetAllConfigs();
    GameConfigurationDB GetConfigurationByName(string name);
    void SaveConfiguration(GameConfigurationDB gameConfiguration);
    void DeleteConfiguration(string name);
}