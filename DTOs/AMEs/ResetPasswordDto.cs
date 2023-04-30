using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.AMEs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string Password { get; set; }
    }
}