using Shared;
using Shared.GameConfigDtos;

namespace DAL.Contracts
{
    public interface IConfigRepository
    {
        Task<List<GameConfigDto>> GetAllUserConfigDto(string userId);
        Task<GameConfigDto> GetConfigurationById(string id);
        Task<Response<GameConfigDto>> CreateGameConfiguration(string userId, GameConfigDto configDto);
        Task<Response> UpdateConfiguration(string configId, GameConfigDto gameConfiguration);
        Task<Response> DeleteConfiguration(string configId);
    }
}