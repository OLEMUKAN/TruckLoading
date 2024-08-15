using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TruckLoadingApp.Models
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        public Guid RouteId { get; set; }

        [ForeignKey("RouteId")]
        public TRoute Route { get; set; }

        [Required]
        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        public ApplicationUser Client { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = "Booked";
    }
}
