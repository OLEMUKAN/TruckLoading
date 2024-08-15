using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TruckLoadingApp.Data;
using TruckLoadingApp.Models;

namespace TruckLoadingApp.Pages.Troutes
{
    public class DeleteModel : PageModel
    {
        private readonly TruckLoadingApp.Data.ApplicationDbContext _context;

        public DeleteModel(TruckLoadingApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TRoute TRoute { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var troute = await _context.TRoutes.FirstOrDefaultAsync(m => m.RouteId == id);

            if (troute == null)
            {
                return NotFound();
            }
            else
            {
                TRoute = troute;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var troute = await _context.TRoutes.FindAsync(id);
            if (troute != null)
            {
                TRoute = troute;
                _context.TRoutes.Remove(TRoute);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
