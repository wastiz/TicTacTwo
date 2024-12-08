using DAL;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class LoadGame : PageModel
{
    private GameRepositoryDb _gameRepository;
    [BindProperty] public string GameId { get; set; }
    public List<GameStateDto> Games { get; set; }


    public LoadGame(AppDbContext context)
    {
        _gameRepository = new GameRepositoryDb();
    }
    
    public void OnGet()
    {
        Games = _gameRepository.GetAllStateDto();
    }

    public IActionResult OnPost()
    {
        var selectedGame = _gameRepository.GetGameStateById(GameId);
    
        if (selectedGame != null)
        {
            return RedirectToPage("/Game", new { gameId = selectedGame.Id, gameName = selectedGame.Name });
        }
        return Page();
    }

}