using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMed_Razor.Entities
{
    public class ModulePreReqs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModulePreReqsId { get; set; }
        public int ModuleId { get; set; }
        public int PreReqId { get; set; }
        public Module Module { get; set; }
        public Module PreReq { get; set; }


    }
}
