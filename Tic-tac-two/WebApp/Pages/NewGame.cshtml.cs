using DAL;
using DAL.DTO;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    private ConfigRepositoryDb _configRepository;
    [BindProperty] public string GameMode { get; set; }
    [BindProperty] public string ConfigId { get; set; }
    public List<GameConfigDto> GameConfigs { get; set; }

    public NewGame(AppDbContext context)
    {
        _configRepository = new ConfigRepositoryDb();
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
            gameBrain.SaveGame(gameId);
            return RedirectToPage("/Game", new { gameId = gameId });
        }
        
        return Page();
    }
}