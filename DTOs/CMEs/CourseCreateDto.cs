using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.CMEs
{
    public class CourseCreateDto
    {
        [Required]
        public List<int>? ModulesList { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
