using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public int NumStudents { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int OrgId { get; set; }
        public Organization Organization { get; set; }

    }
}
