using System.ComponentModel.DataAnnotations;

namespace CommunicationCoverageSupport.Models.Entities
{
    public class Acc
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;
    }
}
