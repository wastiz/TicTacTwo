namespace DAL
{
    using System.Text.Json;
    public class ConfigRepositoryDb : IConfigRepository
    {
        private readonly AppDbContext _context;

        public ConfigRepositoryDb()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            CheckAndCreateInitialConfig();
        }
        
        private void CheckAndCreateInitialConfig()
        {
            if (!_context.GameConfigurations.Any())
            {
                List<GameConfigurationDB> initialConfigurations = new List<GameConfigurationDB>()
                {
                    new GameConfigurationDB() { Name = "Classical" },
                    new GameConfigurationDB() {
                        Name = "Big Game",
                        BoardSizeWidth = 10,
                        BoardSizeHeight = 10,
                        MovableBoardWidth = 5,
                        MovableBoardHeight = 5,
                        ChipsCount = JsonSerializer.Serialize(new int[] { 0, 6, 6 }),
                        WinCondition = 4,
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
                .Select(dbConfig => ConvertFromDbModel(dbConfig))
                .ToList();
        }
        
        public GameConfiguration GetConfigurationByName(string name)
        {
            var configDb = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == name);
            if (configDb == null)
            {
                throw new KeyNotFoundException($"Configuration '{name}' not found.");
            }
            return ConvertFromDbModel(configDb);
        }
        
        public void SaveConfiguration(GameConfiguration gameConfiguration)
        {
            var configDb = ConvertToDbModel(gameConfiguration);
            var existingConfig = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == configDb.Name);

            if (existingConfig == null)
            {
                _context.GameConfigurations.Add(configDb);
            }
            else
            {
                _context.Entry(existingConfig).CurrentValues.SetValues(configDb);
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
        
        private GameConfiguration ConvertFromDbModel(GameConfigurationDB dbConfig)
        {
            return new GameConfiguration
            {
                Name = dbConfig.Name,
                BoardSizeWidth = dbConfig.BoardSizeWidth,
                BoardSizeHeight = dbConfig.BoardSizeHeight,
                MovableBoardWidth = dbConfig.MovableBoardWidth,
                MovableBoardHeight = dbConfig.MovableBoardHeight,
                ChipsCount = JsonSerializer.Deserialize<int[]>(dbConfig.ChipsCount) ?? Array.Empty<int>(),
                WinCondition = dbConfig.WinCondition,
                OptionsAfterNMoves = dbConfig.OptionsAfterNMoves
            };
        }
        
        private GameConfigurationDB ConvertToDbModel(GameConfiguration jsonConfig)
        {
            return new GameConfigurationDB
            {
                Name = jsonConfig.Name,
                BoardSizeWidth = jsonConfig.BoardSizeWidth,
                BoardSizeHeight = jsonConfig.BoardSizeHeight,
                MovableBoardWidth = jsonConfig.MovableBoardWidth,
                MovableBoardHeight = jsonConfig.MovableBoardHeight,
                ChipsCount = JsonSerializer.Serialize(jsonConfig.ChipsCount),
                WinCondition = jsonConfig.WinCondition,
                OptionsAfterNMoves = jsonConfig.OptionsAfterNMoves
            };
        }
    }
}
