namespace CommunicationCoverageSupport.Models.DTOs.SimCards
{
    public class CreateSimCardDto
    {
        public string Iccid { get; set; }
        public long Imsi { get; set; }
        public long Msisdn { get; set; }

        public bool Produced { get; set; }
        public bool Installed { get; set; }
        public bool Purged { get; set; }

        public string Kl1 { get; set; }
        public short Pin1 { get; set; }
        public short Pin2 { get; set; }
        public int Puk1 { get; set; }
        public int Puk2 { get; set; }
        public long Adm1 { get; set; }

        public byte ArtworkId { get; set; }
        public byte AccId { get; set; }

        // Optional: don't allow frontend to set owner, take from JWT instead
    }
}
