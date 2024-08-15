using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TruckLoadingApp.Models;
using System.Threading.Tasks;
using TruckLoadingApp.Data;
using System;

namespace TruckLoadingApp.Pages.Troutes
{
    [Authorize(Roles = "Client")]
    public class BookModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TRoute Route { get; set; }

        public bool IsBooked { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Route = await _context.TRoutes.FindAsync(id);
            if (Route == null)
            {
                return NotFound();
            }

            IsBooked = await _context.Bookings.AnyAsync(b => b.RouteId == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var route = await _context.TRoutes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            var booking = new Booking
            {
                RouteId = route.RouteId,
                ClientId = user.Id,
                BookingDate = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            Message = "Thank you for booking the truck. You will be contacted by the driver shortly.";
            IsBooked = true;

            return Page();
        }
    }
}