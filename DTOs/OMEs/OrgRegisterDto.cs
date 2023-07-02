using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.OMEs
{
    public class OrgRegisterDto
    {
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string WebsiteUrl { get; set; }
        [Required]
        public string Services { get; set; }
        [Required]
        public string WorkType { get; set; }
        [Required]
        public string Email { get; set; }

        //POINT PERSON DETAILS
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string CNIC { get; set; }
        //[Required]
        public string DOB { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
