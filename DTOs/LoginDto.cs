using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Identifier { get; set; }   // Email OR Mobile

        [Required]
        public string Password { get; set; }
    }
}