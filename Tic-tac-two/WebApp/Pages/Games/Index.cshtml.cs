using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;

namespace WebApp.Pages_Games
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<GameSessionDB> GameSessionDB { get;set; } = default!;

        public async Task OnGetAsync()
        {
            GameSessionDB = await _context.GameSessions
                .Include(g => g.GameState)
                .Include(g => g.Player1)
                .Include(g => g.Player2).ToListAsync();
        }
    }
}
