using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        //int[pk, increment]
        public string Name { get; set; }
        public List<ModulePreReqs> ModulePreReqs { get; set; }
        //int[][ref: < Modules.mod_id]
        public int maxDurationHours { get; set; }


        //int
    }
}
