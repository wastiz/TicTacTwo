using DAL.DTO;
using System.Collections.Generic;
using DAL.Contracts.DTO;
using DAL.DTO.GameConfigDtos;

namespace DAL.Contracts
{
    public interface IConfigRepository
    {
        Task<List<GameConfig>> GetAllUserConfigDto(string userId);
        Task<GameConfig> GetConfigurationById(string id);
        Task<Response> CreateGameConfiguration(GameConfiguration config);
        Task<Response> UpdateConfiguration(string id, GameConfiguration gameConfiguration);
        Task<Response> DeleteConfiguration(string configId);
    }
}