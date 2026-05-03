using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.DTOs
{
    public class CreateSocietyDto
    {
        [Required]
        public string Name { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }
    }
}