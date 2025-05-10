using System.ComponentModel.DataAnnotations;

namespace CommunicationCoverageSupport.Models.Entities
{
    public class Artwork
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;
    }
}
