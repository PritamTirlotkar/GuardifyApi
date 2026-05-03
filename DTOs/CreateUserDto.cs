using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; }

        public string? Email { get; set; }
        public string? Mobile { get; set; }

        [Required]
        public string Role { get; set; }
        // Admin / Resident / Guard ONLY (validated in backend)

        public int SocietyId { get; set; }
        public int? ResidenceId { get; set; }
    }
}