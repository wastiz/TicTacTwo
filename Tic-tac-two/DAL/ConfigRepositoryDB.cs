namespace DAL
{
    public class ConfigRepositoryDb : IConfigRepository
    {
        private readonly GameConfigDbContext _context;

        public ConfigRepositoryDb()
        {
            _context = new GameConfigDbContext();
            _context.Database.EnsureCreated();
        }

        public List<string> GetAllConfigNames()
        {
            return _context.GameConfigurations.Select(gc => gc.Name).ToList();
        }

        public List<GameConfigurationDB> GetAllConfigs()
        {
            return _context.GameConfigurations.ToList();
        }

        public GameConfigurationDB GetConfigurationByName(string name)
        {
            var config = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == name);
            if (config == null)
            {
                throw new KeyNotFoundException($"Configuration '{name}' not found.");
            }
            return config;
        }

        public void SaveConfiguration(GameConfigurationDB gameConfigurationDB)
        {
            var existingConfig = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == gameConfigurationDB.Name);
            if (existingConfig == null)
            {
                _context.GameConfigurations.Add(gameConfigurationDB);
            }
            else
            {
                _context.Entry(existingConfig).CurrentValues.SetValues(gameConfigurationDB); // Обновляем поля
            }
            _context.SaveChanges();
        }

        public void DeleteConfiguration(string name)
        {
            var config = _context.GameConfigurations.SingleOrDefault(gc => gc.Name == name);
            if (config == null)
            {
                throw new KeyNotFoundException($"Configuration '{name}' not found.");
            }
            _context.GameConfigurations.Remove(config);
            _context.SaveChanges();
        }
    }
}