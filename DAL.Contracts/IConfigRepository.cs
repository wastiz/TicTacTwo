using DAL.Contracts.DTO;
using Shared.GameConfigDtos;

namespace DAL.Contracts
{
    public interface IConfigRepository
    {
        Task<List<GameConfigDto>> GetAllUserConfigDto(string userId);
        Task<GameConfigDto> GetConfigurationById(string id);
        Task<Response> CreateGameConfiguration(string userId, GameConfigDto configDto);
        Task<Response> UpdateConfiguration(string configId, GameConfigDto gameConfiguration);
        Task<Response> DeleteConfiguration(string configId);
    }
}