using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.DTOs
{
    public class SetPasswordDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}