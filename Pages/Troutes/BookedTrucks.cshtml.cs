using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TruckLoadingApp.Data;
using TruckLoadingApp.Models;

namespace TruckLoadingApp.Pages.Troutes
{
    [Authorize(Roles = "Client")]
    public class BookedTrucksModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookedTrucksModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<BookedTruckViewModel> BookedTrucks { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var bookedTrucks = await _context.Bookings
                .Include(b => b.Route)
                .ThenInclude(r => r.Driver)
                .Where(b => b.ClientId == user.Id)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            BookedTrucks = bookedTrucks.Select(b => new BookedTruckViewModel
            {
                Booking = b,
                DriverFirstName = b.Route.Driver.FirstName
            }).ToList();

            return Page();
        }
    }

    public class BookedTruckViewModel
    {
        public Booking Booking { get; set; }
        public string DriverFirstName { get; set; }
    }
}