using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationCoverageSupport.Models.Entities
{
    public class SimCard
    {
        [Key]
        [MaxLength(20)]
        public string Iccid { get; set; } = null!;

        [Required]
        public long Imsi { get; set; }

        [Required]
        public long Msisdn { get; set; }

        public bool Produced { get; set; } = false;
        public bool Installed { get; set; } = false;
        public bool Purged { get; set; } = false;

        [Required]
        [MaxLength(255)]
        public string Kl1 { get; set; } = null!;

        [Required]
        public short Pin1 { get; set; }

        [Required]
        public short Pin2 { get; set; }

        [Required]
        public int Puk1 { get; set; }

        [Required]
        public int Puk2 { get; set; }

        [Required]
        public long Adm1 { get; set; }

        // FK to artwork
        public byte ArtworkId { get; set; }
        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; } = null!;

        // FK to acc
        public byte AccId { get; set; }
        [ForeignKey("AccId")]
        public Acc Acc { get; set; } = null!;

        // FK to ApplicationUser (card owner)
        [Required]
        public int CardOwnerId { get; set; }

        [ForeignKey("CardOwnerId")]
        public ApplicationUser CardOwner { get; set; } = null!;
    }
}
