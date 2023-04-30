using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        public string Name { get; set; }
        public List<CourseModules> ModulesList { get; set; }

        //int[pk, increment]
        //public List<Module> Modules { get; set; }
        //int[][ref: < Module_Assignments.assign_id]

        //public List<int> Course_pre_req { get; set; }
        //int[][ref: < Courses.course_id]
        //public List<Course> CoursePreReq { get; set; }
        //public List<Module> ModulesPreReq { get; set; }
        //int[][ref: < Modules.mod_id]

        //public int OrgId { get; set; }
        //public virtual Organization Organization { get; set; }
        //public ICollection<Session> Sessions { get; set; }
    }
}
