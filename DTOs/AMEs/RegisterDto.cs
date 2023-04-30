using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.AMEs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string CNIC { get; set; }
        //[Required]
        public string DOB { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Password { get; set; }
        public bool IsLecturer { get; set; } = false;
    }
}