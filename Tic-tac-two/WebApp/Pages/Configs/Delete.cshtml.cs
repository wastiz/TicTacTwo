using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;

namespace WebApp.Pages_Configs
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GameConfigurationDB GameConfigurationDB { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameconfigurationdb = await _context.GameConfigurations.FirstOrDefaultAsync(m => m.Id == id);

            if (gameconfigurationdb is not null)
            {
                GameConfigurationDB = gameconfigurationdb;

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

            var gameconfigurationdb = await _context.GameConfigurations.FindAsync(id);
            if (gameconfigurationdb != null)
            {
                GameConfigurationDB = gameconfigurationdb;
                _context.GameConfigurations.Remove(GameConfigurationDB);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
