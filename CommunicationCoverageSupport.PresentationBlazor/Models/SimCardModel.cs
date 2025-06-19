using System.ComponentModel.DataAnnotations;

namespace CommunicationCoverageSupport.PresentationBlazor.Models
{
    public class SimCardModel
    {
        [Required] public string Iccid { get; set; } = string.Empty;
        [Required] public string Imsi { get; set; } = string.Empty;
        [Required] public string Msisdn { get; set; } = string.Empty;
        public int KIndId { get; set; }
        [Required] public string Ki { get; set; } = string.Empty;
        public short Pin1 { get; set; }
        public short Pin2 { get; set; }
        public int Puk1 { get; set; }
        public int Puk2 { get; set; }
        [Required] public string Adm1 { get; set; } = string.Empty;
        public int ArtworkIdInt { get; set; }
        public int AccIdInt { get; set; }
        public bool Installed { get; set; }
        public long CardOwnerId { get; set; }
    }
}
