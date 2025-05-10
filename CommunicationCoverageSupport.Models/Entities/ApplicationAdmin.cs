using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationCoverageSupport.Models.Entities
{
    public class ApplicationAdmin
    {
        [Key]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
    }
}
