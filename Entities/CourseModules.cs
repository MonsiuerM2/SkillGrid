using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class CourseModules
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseModulesId { get; set; }
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public Module? Module { get; set; }
        public Course? Course { get; set; }
    }
}
