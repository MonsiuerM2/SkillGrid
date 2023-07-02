using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrgId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string WebsiteUrl { get; set; }
        [Required]
        public string Services { get; set; }
        [Required]
        public string WorkType { get; set; }
    }
}
