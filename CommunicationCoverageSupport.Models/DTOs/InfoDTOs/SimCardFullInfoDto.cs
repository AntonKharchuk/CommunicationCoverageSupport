using CommunicationCoverageSupport.Models.DTOs;


namespace CommunicationCoverageSupport.Models.DTOs.InfoDTOs
{
    public class SimCardFullInfoDto
    {
        public SimCardDto SimCard { get; set; } = null!;
        public ArtworkDto Artwork { get; set; } = null!;
        public AccDto Acc { get; set; } = null!;
        public OwnerDto Owner { get; set; } = null!;
        public TransportKeyDto TransportKey { get; set; } = null!;
        public HlrStatusDto HlrStatus { get; set; } = null!;
    }

}