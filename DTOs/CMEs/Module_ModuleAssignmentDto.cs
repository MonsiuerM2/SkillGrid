using DMed_Razor.Entities;

namespace DMed_Razor.DTOs.CMEs
{
    public class Module_ModuleAssignmentDto
    {
        public Module? Module { get; set; }
        public List<ModuleAssignment>? ModuleAssignments { get; set; }
    }
}
