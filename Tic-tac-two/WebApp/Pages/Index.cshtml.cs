using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly GameStateDbContext _ctx;

    public IndexModel(ILogger<IndexModel> logger, GameStateDbContext _ctx)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}