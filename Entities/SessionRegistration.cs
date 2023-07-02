using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class SessionRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionRegId { get; set; }
        public int StudentId { get; set; }
        public AppUser? Student { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;
    }
}
