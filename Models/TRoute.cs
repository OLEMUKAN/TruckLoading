using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TruckLoadingApp.Models
{
    public class TRoute
    {
        [Key]
        public Guid RouteId { get; set; }

        [Required]
        [StringLength(100)]
        public string Origin { get; set; }

        [Required]
        [StringLength(100)]
        public string Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Available Date")]
        public DateTime AvailableDate { get; set; }

        // Remove [Required] from DriverId
        public string DriverId { get; set; }

        [ForeignKey("DriverId")]
        public ApplicationUser? Driver { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
