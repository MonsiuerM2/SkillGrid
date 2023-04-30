using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.AMEs
{
    public class UserrnameDto
    {
        [Required]
        public string Username { get; set; }
    }
}