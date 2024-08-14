using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TruckLoadingApp.Models
{
    public class Truck
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "Capacity (Tons)")]
        public double Capacity { get; set; }

        [Required]
        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey("ApplicationUser")]
        public string DriverId { get; set; }

        public ApplicationUser? Driver { get; set; }
    }
}
