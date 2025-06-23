using DAL.Contracts;
using DAL.Contracts.DTO;
using DAL.DTO;
using DAL.DTO.GameConfigDtos;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    using System.Text.Json;
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
                    OptionsAfterNMoves = 3,
                });
            }

            if (initialConfigurations.Count > 0)
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
                    
                })
                .ToListAsync();
        }

        
        public async Task<GameConfig> GetConfigurationById(string id)
        {
            var configDb = await _context.GameConfigurations.SingleOrDefaultAsync(gc => gc.Id == id);
            if (configDb == null)
            {
                throw new KeyNotFoundException($"Configuration '{id}' not found.");
            }
            return configDb;
        }

        public async Task<Response> CreateGameConfiguration(GameConfiguration config)
        {
            try
            {
                await _context.GameConfigurations.AddAsync(config);
                await _context.SaveChangesAsync();

                return Response<GameConfiguration>.Ok(config, "Game configuration created");
            }
            catch (Exception ex)
            {
                return Response<GameConfiguration>.Fail($"Error creating game configuration: {ex.Message}");
            }
        }

        
        public void UpdateConfiguration(string id, GameConfiguration gameConfiguration)
        {
            var existingConfig = _context.GameConfigurations.SingleOrDefault(gc => gc.Id == id);

            if (existingConfig == null)
            {
                _context.GameConfigurations.Add(gameConfiguration);
            }
            else
            {
                _context.Entry(existingConfig).CurrentValues.SetValues(gameConfiguration);
            }

            _context.SaveChanges();
        }
        
        public void DeleteConfiguration(string configId)
        {
            var config = _context.GameConfigurations.SingleOrDefault(gc => gc.Id == configId);
            
            if (config == null)
            {
                throw new KeyNotFoundException($"Configuration '{configId}' not found.");
            }

            _context.GameConfigurations.Remove(config);
            _context.SaveChanges();
        }
        
    }
}
