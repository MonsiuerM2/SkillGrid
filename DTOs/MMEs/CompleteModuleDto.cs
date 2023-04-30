using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.CMEs
{
    public class CompleteModuleDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int LectureId { get; set; }
    }
}
