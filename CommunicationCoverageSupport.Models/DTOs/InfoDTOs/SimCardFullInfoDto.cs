using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs.InfoDTOs
{
    public class SimCardFullInfoDto
    {
        public SimCardDto SimCard { get; set; } = new();
        public ArtworkDto Artwork { get; set; } = new();
        public AccDto Acc { get; set; } = new();
        public OwnerDto Owner { get; set; } = new();
    }
}
