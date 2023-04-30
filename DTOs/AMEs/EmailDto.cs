using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.AMEs
{
    public class EmailDto
    {
        [Required]
        public string Username { get; set; }

        public string Email { get; set; }
    }
}