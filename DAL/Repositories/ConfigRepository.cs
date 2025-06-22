using DAL.Contracts;
using DAL.DTO;

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
            if (!_context.GameConfigurations.Any())
            {
                List<GameConfiguration> initialConfigurations = new List<GameConfiguration>()
                {
                    new GameConfiguration() { Id = "classic", Name = "Classical" },
                    new GameConfiguration() {
                        Id = "big-game",
                        Name = "Big Game",
                        BoardSizeWidth = 10,
                        BoardSizeHeight = 10,
                        MovableBoardWidth = 5,
                        MovableBoardHeight = 5,
                        ChipsCount = new int[] { 0, 6, 6 },
                        WinCondition = 3,
                        OptionsAfterNMoves = 3,
                    },
                };
                
                _context.GameConfigurations.AddRange(initialConfigurations);
                _context.SaveChanges();
            }
        }
        
        public List<GameConfiguration> GetAllConfigs()
        {
            return _context.GameConfigurations
                .Select(dbConfig => dbConfig)
                .ToList();
        }
        
        public List<GameConfigDto> GetAllUserConfigDto(string userId)
        {
            return _context.GameConfigurations
                .Where(gc => gc.CreatedBy == userId)
                .Select(gc => new GameConfigDto
                {
                    ConfigId = gc.Id,
                    ConfigName = gc.Name
                })
                .ToList();
        }

        
        public GameConfiguration GetConfigurationById(string id)
        {
            var configDb = _context.GameConfigurations.SingleOrDefault(gc => gc.Id == id);
            if (configDb == null)
            {
                throw new KeyNotFoundException($"Configuration '{id}' not found.");
            }
            return configDb;
        }

        public void CreateGameConfiguration(GameConfiguration config)
        {
            _context.GameConfigurations.Add(config);
            _context.SaveChanges();
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
