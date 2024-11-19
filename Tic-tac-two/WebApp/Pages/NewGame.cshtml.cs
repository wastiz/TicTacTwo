using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    private ConfigRepositoryDb _repository;

    public NewGame(AppDbContext context)
    {
        _repository = new ConfigRepositoryDb();
    }
    
    public List<string> GameConfigs { get; set; } = new List<string>();
    public void OnGet() 
    {
        GameConfigs = _repository.GetAllConfigNames();
    }
    [BindProperty]
    public string GameMode { get; set; }
    [BindProperty]
    public string GameConfig { get; set; }

    public IActionResult OnPost()
    {
        TempData["GameMode"] = GameMode;
        TempData["GameConfig"] = GameConfig;
        return RedirectToPage("/Game", new { mode = GameMode, config = GameConfig });
    }
}