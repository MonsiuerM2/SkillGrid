using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class ModuleAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }
        //int[pk, increment]
        public int ModuleId { get; set; }
        //int[ref: > Modules.mod_id]
        public virtual Module Module { get; set; }
        public int LecturerId { get; set; }
        //int[ref: > Teachers.tch_id]
        public virtual AppUser Lecturer { get; set; }
    }
}
