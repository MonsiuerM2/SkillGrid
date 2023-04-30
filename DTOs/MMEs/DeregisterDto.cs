using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.CMEs
{
    public class DeregisterDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int LectureId { get; set; }
    }
}
