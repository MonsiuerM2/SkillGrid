using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegId { get; set; }
        //int[pk, increment]
        public int LectureId { get; set; }
        //int
        public bool isModule { get; set; }
        //bool
        public int StudentId { get; set; }
        public virtual AppUser Student { get; set; }
        //int[ref: > Students.std_id]
        public bool Completed { get; set; } = false;
        //bool[default: false]
        public DateTime StartDate { get; set; }
        //date[default: `now()`]
        public DateTime EndDate { get; set; }
        //date
    }
}
