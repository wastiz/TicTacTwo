using DAL.Contracts;
using DAL.Mappers;
using Domain;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.GameConfigDtos;

namespace DAL
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly AppDbContext _context;

        public ConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GameConfigDto>> GetAllUserConfigDto(string userId)
        {
            return await _context.GameConfigurations
                .Where(gc => gc.CreatedBy == userId)
                .Select(gc => new GameConfigDto
                {
                    Id = gc.Id,
                    Name = gc.Name,
                    BoardSizeWidth = gc.BoardSizeWidth,
                    BoardSizeHeight = gc.BoardSizeHeight,
                    MovableBoardWidth = gc.MovableBoardWidth,
                    MovableBoardHeight = gc.MovableBoardHeight,
                    Player1Chips = gc.Player1Chips,
                    Player2Chips = gc.Player2Chips,
                    WinCondition = gc.WinCondition,
                    AbilitiesAfterNMoves = gc.AbilitiesAfterNMoves
                })
                .ToListAsync();
        }

        public async Task<GameConfigDto> GetConfigurationById(string id)
        {
            var config = await _context.GameConfigurations.SingleOrDefaultAsync(gc => gc.Id == id);
            if (config == null)
                throw new KeyNotFoundException($"Configuration '{id}' not found.");

            return new GameConfigDto
            {
                Id = config.Id,
                Name = config.Name,
                BoardSizeWidth = config.BoardSizeWidth,
                BoardSizeHeight = config.BoardSizeHeight,
                MovableBoardWidth = config.MovableBoardWidth,
                MovableBoardHeight = config.MovableBoardHeight,
                Player1Chips = config.Player1Chips,
                Player2Chips = config.Player2Chips,
                WinCondition = config.WinCondition,
                AbilitiesAfterNMoves = config.AbilitiesAfterNMoves
            };
        }

        public async Task<Response<GameConfigDto>> CreateGameConfiguration(string userId, GameConfigDto configDto)
        {
            try
            {
                var config = ConfigMapper.ToDomain(configDto);
                config.CreatedBy = userId;

                await _context.GameConfigurations.AddAsync(config);
                await _context.SaveChangesAsync();

                return new Response<GameConfigDto>()
                {
                    Success = true,
                    Message = "Game configuration created successfully.",
                    Data = configDto
                };
            }
            catch (Exception ex)
            {
                return Response<GameConfigDto>.Fail($"Error creating game configuration: {ex.Message}");
            }
        }

        public async Task<Response<GameConfigDto>> UpdateConfiguration(string id, GameConfigDto dto)
        {
            if (string.IsNullOrEmpty(id))
                return Response<GameConfigDto>.Fail("Id cannot be empty");
    
            if (dto == null)
                return Response<GameConfigDto>.Fail("Configuration data is required");

            try
            {
                var existingConfig = await _context.GameConfigurations
                    .SingleOrDefaultAsync(gc => gc.Id == id);
            
                if (existingConfig == null)
                {
                    return Response<GameConfigDto>.Fail($"Configuration '{id}' not found.");
                }
                
                existingConfig.Name = dto.Name;
                existingConfig.BoardSizeWidth = dto.BoardSizeWidth;
                existingConfig.BoardSizeHeight = dto.BoardSizeHeight;
                existingConfig.MovableBoardWidth = dto.MovableBoardWidth;
                existingConfig.MovableBoardHeight = dto.MovableBoardHeight;
                existingConfig.Player1Chips = dto.Player1Chips;
                existingConfig.Player2Chips = dto.Player2Chips;
                existingConfig.WinCondition = dto.WinCondition;
                existingConfig.AbilitiesAfterNMoves = dto.AbilitiesAfterNMoves;

                await _context.SaveChangesAsync();
                
                return Response<GameConfigDto>.Ok(ConfigMapper.ToDto(existingConfig), "Configuration updated successfully");
            }
            catch (Exception ex)
            {
                return Response<GameConfigDto>.Fail($"Error updating configuration: {ex.Message}");
            }
        }

        public async Task<Response> DeleteConfiguration(string configId)
        {
            var config = await _context.GameConfigurations.SingleOrDefaultAsync(gc => gc.Id == configId);
            if (config == null)
                return Response.Fail($"Configuration '{configId}' not found.");

            _context.GameConfigurations.Remove(config);
            await _context.SaveChangesAsync();
            return Response.Ok("Configuration deleted");
        }
    }
}
