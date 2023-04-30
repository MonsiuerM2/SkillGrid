using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.CMEs
{
    public class ModAssignDto
    {
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int LecturerId { get; set; }
    }
}
