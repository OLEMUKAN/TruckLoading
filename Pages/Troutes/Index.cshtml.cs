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
    [Authorize(Roles = "Client,Driver")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<TRouteViewModel> TRoutes { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string OriginFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DestinationFilter { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var query = _context.TRoutes.Include(t => t.Driver).AsQueryable();

            if (User.IsInRole("Driver"))
            {
                // Filter routes for the logged-in driver
                query = query.Where(t => t.Driver.Id == user.Id);
            }

            if (!string.IsNullOrEmpty(OriginFilter))
            {
                query = query.Where(t => t.Origin.Contains(OriginFilter));
            }

            if (!string.IsNullOrEmpty(DestinationFilter))
            {
                query = query.Where(t => t.Destination.Contains(DestinationFilter));
            }

            var routes = await query.ToListAsync();

            TRoutes = routes.Select(r => new TRouteViewModel
            {
                Route = r,
                DriverFirstName = r.Driver.FirstName,
                IsBooked = _context.Bookings.Any(b => b.RouteId == r.RouteId)
            }).ToList();
        }
    }

    public class TRouteViewModel
    {
        public TRoute Route { get; set; }
        public string DriverFirstName { get; set; }
        public bool IsBooked { get; set; }
    }
}