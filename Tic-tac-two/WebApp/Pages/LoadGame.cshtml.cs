using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class LoadGame : PageModel
{
    private GameRepositoryDb _gameRepository;
    [BindProperty] public string GameName { get; set; }
    public List<string> Games { get; set; } = new List<string>();

    public LoadGame(AppDbContext context)
    {
        _gameRepository = new GameRepositoryDb();
    }
    
    public void OnGet()
    {
        Games = _gameRepository.GetAllStateNames();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/Game", new { gameId = _gameRepository.GetGameStateByName(GameName) });
    }
}