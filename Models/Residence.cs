using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.Models
{
    public class Residence
    {
        [Key]
        public int Id { get; set; }

        // 🏢 Link to Society (multi-tenant system)
        [Required]
        public int SocietyId { get; set; }

        // 🏗 Building details
        [Required]
        [MaxLength(100)]
        public string BuildingName { get; set; }

        [MaxLength(50)]
        public string? Wing { get; set; }

        // 🏠 Flat details
        [Required]
        [MaxLength(20)]
        public string FlatNumber { get; set; }

        [MaxLength(10)]
        public string? Floor { get; set; }

        // 📍 Optional address (if needed for mapping)
        [MaxLength(200)]
        public string? AddressLine { get; set; }

        // 👤 Link to resident user (optional)
        public int? UserId { get; set; }
    }
}