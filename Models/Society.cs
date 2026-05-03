using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.Models
{
    public class Society
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}