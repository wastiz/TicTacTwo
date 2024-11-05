namespace DAL
{
    using System.Text.Json;
    public class ConfigRepositoryDb : IConfigRepository
    {
        private readonly GameConfigDbContext _context;

        public ConfigRepositoryDb()
        {
            _context = new GameConfigDbContext();
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
                        ChipsCount = new int[] { 0, 6, 6 },
                        WinCondition = 4,
                        MovePieceAfterNMoves = 3,
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
                .Select(dbConfig => DbToDomainConfig(dbConfig))
                .ToList();
        }
        
        public GameConfiguration GetConfigurationByName(string name)
        {
            var configDb = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == name);
            if (configDb == null)
            {
                throw new KeyNotFoundException($"Configuration '{name}' not found.");
            }
            return DbToDomainConfig(configDb);
        }
        
        public void SaveConfiguration(GameConfiguration gameConfiguration)
        {
            var configDb = DomainToDbConfig(gameConfiguration);
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
        
        private GameConfiguration DbToDomainConfig(GameConfigurationDB dbConfig)
        {
            return new GameConfiguration
            {
                Name = dbConfig.Name,
                BoardSizeWidth = dbConfig.BoardSizeWidth,
                BoardSizeHeight = dbConfig.BoardSizeHeight,
                MovableBoardWidth = dbConfig.MovableBoardWidth,
                MovableBoardHeight = dbConfig.MovableBoardHeight,
                ChipsCount = JsonSerializer.Deserialize<int[]>(dbConfig.ChipsCountSerialized) ?? Array.Empty<int>(),
                WinCondition = dbConfig.WinCondition,
                MovePieceAfterNMoves = dbConfig.MovePieceAfterNMoves
            };
        }

        // Преобразование GameConfiguration в GameConfigurationDB
        private GameConfigurationDB DomainToDbConfig(GameConfiguration domainConfig)
        {
            return new GameConfigurationDB
            {
                Name = domainConfig.Name,
                BoardSizeWidth = domainConfig.BoardSizeWidth,
                BoardSizeHeight = domainConfig.BoardSizeHeight,
                MovableBoardWidth = domainConfig.MovableBoardWidth,
                MovableBoardHeight = domainConfig.MovableBoardHeight,
                ChipsCountSerialized = JsonSerializer.Serialize(domainConfig.ChipsCount),
                WinCondition = domainConfig.WinCondition,
                MovePieceAfterNMoves = domainConfig.MovePieceAfterNMoves
            };
        }
    }
}
