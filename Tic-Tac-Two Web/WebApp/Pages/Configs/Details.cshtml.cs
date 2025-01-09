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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public GameConfiguration GameConfiguration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameconfigurationdb = await _context.GameConfigurations.FirstOrDefaultAsync(m => m.Id == id);

            if (gameconfigurationdb is not null)
            {
                GameConfiguration = gameconfigurationdb;

                return Page();
            }

            return NotFound();
        }
    }
}
