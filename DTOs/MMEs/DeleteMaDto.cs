using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.CMEs
{
    public class DeleteMaDto
    {
        [Required]
        public int AssignmentId { get; set; }
    }
}
