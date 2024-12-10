using DAL;
using DAL.DTO;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    private ConfigRepositoryDb _configRepository;
    private AppDbContext _context;
    [BindProperty] public string GameMode { get; set; }
    [BindProperty] public string ConfigId { get; set; }
    public List<GameConfigDto> GameConfigs { get; set; }

    public NewGame(AppDbContext context)
    {
        _configRepository = new ConfigRepositoryDb();
        _context = context;
    }

    public void OnGet()
    {
        GameConfigs = _configRepository.GetAllConfigDto();
    }

    public IActionResult OnPost()
    {
        var selectedConfig = _configRepository.GetConfigurationById(ConfigId);
        if (selectedConfig != null)
        {
            Brain gameBrain = new Brain(selectedConfig);
            string gameId = Guid.NewGuid().ToString();
            string sessionId = Guid.NewGuid().ToString();
            GameSessionDB newSession = new GameSessionDB()
            {
                Id = sessionId,
                GameStateId = gameId,
                Player1Id = TempData["UserId"].ToString(),
                GameMode = GameMode,
                GamePassword = GenerateNumericPassword(6)
            };
            gameBrain.SaveGame(gameId);
            _context.GameSessions.Add(newSession);
            _context.SaveChanges();
            if (GameMode == "two-players")
            {
                return RedirectToPage("/Game" , new { sessionId = sessionId });
            }
            else if (GameMode == "two-players-online")
            {
                return RedirectToPage("/GameOnline", new { sessionId = sessionId });
            }
            else
            {
                return Page();
            }
        }
        
        return Page();
    }
    
    static string GenerateNumericPassword(int length)
    {
        Random random = new Random();
        string password = "";

        for (int i = 0; i < length; i++)
        {
            password += random.Next(0, 10);
        }

        return password;
    }
}