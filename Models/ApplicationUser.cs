using Microsoft.AspNetCore.Identity;

namespace TruckLoadingApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string District { get; set; }
    }
}
