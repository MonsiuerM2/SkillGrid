using System.ComponentModel.DataAnnotations;

namespace DMed_Razor.DTOs.CMEs
{
    public class ModuleDto
    {
        [Required]
        public string Name { get; set; }

        public List<int>? ModulesPreReq { get; set; }
        //int[][ref: < Modules.mod_id]
        [Required]
        public int MaxDurationHours { get; set; }
    }
}
