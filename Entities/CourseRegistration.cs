using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class CourseRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseRegId { get; set; }
        //int[pk, increment]
        public int CourseId { get; set; }
        //int
        public int StudentId { get; set; }
        public virtual AppUser Student { get; set; }
        //int[ref: > Students.std_id]
        public bool Completed { get; set; } = false;
        //bool[default: false]
        public DateTime CourseEnrolledtDate { get; set; } = DateTime.Now;
        //date[default: `now()`]

    }
}
