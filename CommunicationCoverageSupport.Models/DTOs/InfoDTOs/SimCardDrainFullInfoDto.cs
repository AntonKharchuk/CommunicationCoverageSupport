using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs.InfoDTOs
{
    public class SimCardDrainFullInfoDto
    {
        public SimCardDrainDto SimCardDrain { get; set; } = null!;
        public ArtworkDto Artwork { get; set; } = null!;
        public AccDto Acc { get; set; } = null!;
        public OwnerDto Owner { get; set; } = null!;
        public TransportKeyDto TransportKey { get; set; } = null!;
    }
}
