using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.DTOs
{
    public class CreateResidenceDto
    {
        [Required]
        public int SocietyId { get; set; }

        [Required]
        public string BuildingName { get; set; }

        public string? Wing { get; set; }

        [Required]
        public string FlatNumber { get; set; }

        public string? Floor { get; set; }

        public string? AddressLine { get; set; }
    }
}