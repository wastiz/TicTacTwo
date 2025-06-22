using DAL.DTO;
using System.Collections.Generic;

namespace DAL.Contracts
{
    public interface IConfigRepository
    {
        List<GameConfiguration> GetAllConfigs();
        List<GameConfigDto> GetAllUserConfigDto(string userId);
        GameConfiguration GetConfigurationById(string id);
        void CreateGameConfiguration(GameConfiguration config);
        void UpdateConfiguration(string id, GameConfiguration gameConfiguration);
        void DeleteConfiguration(string configId);
    }
}