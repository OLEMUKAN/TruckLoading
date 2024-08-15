using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TruckLoadingApp.Models;

namespace TruckLoadingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<TRoute> TRoutes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
