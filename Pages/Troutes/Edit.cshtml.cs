using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TruckLoadingApp.Data;
using TruckLoadingApp.Models;

namespace TruckLoadingApp.Pages.Troutes
{
    public class EditModel : PageModel
    {
        private readonly TruckLoadingApp.Data.ApplicationDbContext _context;

        public EditModel(TruckLoadingApp.Data.ApplicationDbContext context)
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

            var troute =  await _context.TRoutes.FirstOrDefaultAsync(m => m.RouteId == id);
            if (troute == null)
            {
                return NotFound();
            }
            TRoute = troute;
           ViewData["DriverId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TRoute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRouteExists(TRoute.RouteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TRouteExists(Guid id)
        {
            return _context.TRoutes.Any(e => e.RouteId == id);
        }
    }
}
