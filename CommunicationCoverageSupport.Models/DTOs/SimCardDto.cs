namespace CommunicationCoverageSupport.Models.DTOs
{
    public class SimCardDto
    {
        public string Iccid { get; set; } = null!;
        public string Imsi { get; set; } = null!;
        public string Msisdn { get; set; } = null!;
        public byte KIndId { get; set; }
        public string Ki1 { get; set; } = null!;
        public short Pin1 { get; set; }
        public short Pin2 { get; set; }
        public int Puk1 { get; set; }
        public int Puk2 { get; set; }
        public string Adm1 { get; set; } = null!;
        public byte ArtworkId { get; set; }
        public byte AccId { get; set; }
        public bool Installed { get; set; }
        public long CardOwnerId { get; set; }
    }
}
