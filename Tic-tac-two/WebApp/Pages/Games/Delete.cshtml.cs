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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GameSessionDB GameSessionDB { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesessiondb = await _context.GameSessions.FirstOrDefaultAsync(m => m.Id == id);

            if (gamesessiondb is not null)
            {
                GameSessionDB = gamesessiondb;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesessiondb = await _context.GameSessions.FindAsync(id);
            if (gamesessiondb != null)
            {
                GameSessionDB = gamesessiondb;
                _context.GameSessions.Remove(GameSessionDB);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
