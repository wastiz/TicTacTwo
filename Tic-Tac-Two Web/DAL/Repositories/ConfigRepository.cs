using DAL.DTO;

namespace DAL
{
    using System.Text.Json;
    public class ConfigRepository
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
                    new GameConfiguration() { Name = "Classical" },
                    new GameConfiguration() {
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
        
        public List<string> GetAllConfigNames()
        {
            return _context.GameConfigurations.Select(gc => gc.Name).ToList();
        }
        
        public List<GameConfiguration> GetAllConfigs()
        {
            return _context.GameConfigurations
                .Select(dbConfig => dbConfig)
                .ToList();
        }
        
        public List<GameConfigDto> GetAllConfigDto()
        {
            return _context.GameConfigurations
                .Select(gs => new GameConfigDto()
                {
                    ConfigId = gs.Id,
                    ConfigName = gs.Name,
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
        
        public void SaveConfiguration(GameConfiguration gameConfiguration)
        {
            var existingConfig = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == gameConfiguration.Name);

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
        
        public void DeleteConfiguration(string name)
        {
            var configDb = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == name);
            if (configDb == null)
            {
                throw new KeyNotFoundException($"Configuration '{name}' not found.");
            }

            _context.GameConfigurations.Remove(configDb);
            _context.SaveChanges();
        }
        
    }
}
