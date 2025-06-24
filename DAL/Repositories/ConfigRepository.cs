using DAL.Contracts;
using DAL.Contracts.DTO;
using DAL.DTO;
using DAL.DTO.GameConfigDtos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DAL
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly AppDbContext _context;

        public ConfigRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            CheckAndCreateInitialConfig();
        }

        private void CheckAndCreateInitialConfig()
        {
            var existingIds = _context.GameConfigurations
                .Select(c => c.Id)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var initialConfigurations = new List<GameConfiguration>();

            if (!existingIds.Contains("classic"))
            {
                initialConfigurations.Add(new GameConfiguration
                {
                    Id = "classic",
                    Name = "Classical"
                });
            }

            if (!existingIds.Contains("big-game"))
            {
                initialConfigurations.Add(new GameConfiguration
                {
                    Id = "big-game",
                    Name = "Big Game",
                    BoardSizeWidth = 10,
                    BoardSizeHeight = 10,
                    MovableBoardWidth = 5,
                    MovableBoardHeight = 5,
                    Player1Chips = 6,
                    Player2Chips = 6,
                    WinCondition = 3,
                    OptionsAfterNMoves = 3
                });
            }

            if (initialConfigurations.Any())
            {
                _context.GameConfigurations.AddRange(initialConfigurations);
                _context.SaveChanges();
            }
        }

        public async Task<List<GameConfig>> GetAllUserConfigDto(string userId)
        {
            return await _context.GameConfigurations
                .Where(gc => gc.CreatedBy == userId)
                .Select(gc => new GameConfig
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
                    OptionsAfterNMoves = gc.OptionsAfterNMoves
                })
                .ToListAsync();
        }

        public async Task<GameConfig> GetConfigurationById(string id)
        {
            var config = await _context.GameConfigurations.SingleOrDefaultAsync(gc => gc.Id == id);
            if (config == null)
                throw new KeyNotFoundException($"Configuration '{id}' not found.");

            return new GameConfig
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
                OptionsAfterNMoves = config.OptionsAfterNMoves
            };
        }

        public async Task<Response> CreateGameConfiguration(string userId, GameConfig configDto)
        {
            try
            {
                var config = new GameConfiguration
                {
                    Id = configDto.Id,
                    Name = configDto.Name,
                    CreatedBy = userId,
                    BoardSizeWidth = configDto.BoardSizeWidth,
                    BoardSizeHeight = configDto.BoardSizeHeight,
                    MovableBoardWidth = configDto.MovableBoardWidth,
                    MovableBoardHeight = configDto.MovableBoardHeight,
                    Player1Chips = configDto.Player1Chips,
                    Player2Chips = configDto.Player2Chips,
                    WinCondition = configDto.WinCondition,
                    OptionsAfterNMoves = configDto.OptionsAfterNMoves
                };

                await _context.GameConfigurations.AddAsync(config);
                await _context.SaveChangesAsync();

                return Response.Ok("Game configuration created");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error creating game configuration: {ex.Message}");
            }
        }

        public async Task<Response> UpdateConfiguration(string id, GameConfig dto)
        {
            var existingConfig = await _context.GameConfigurations.SingleOrDefaultAsync(gc => gc.Id == id);
            if (existingConfig == null)
            {
                return Response.Fail($"Configuration '{id}' not found.");
            }

            existingConfig.Name = dto.Name;
            existingConfig.BoardSizeWidth = dto.BoardSizeWidth;
            existingConfig.BoardSizeHeight = dto.BoardSizeHeight;
            existingConfig.MovableBoardWidth = dto.MovableBoardWidth;
            existingConfig.MovableBoardHeight = dto.MovableBoardHeight;
            existingConfig.Player1Chips = dto.Player1Chips;
            existingConfig.Player2Chips = dto.Player2Chips;
            existingConfig.WinCondition = dto.WinCondition;
            existingConfig.OptionsAfterNMoves = dto.OptionsAfterNMoves;

            await _context.SaveChangesAsync();
            return Response.Ok("Configuration updated");
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
