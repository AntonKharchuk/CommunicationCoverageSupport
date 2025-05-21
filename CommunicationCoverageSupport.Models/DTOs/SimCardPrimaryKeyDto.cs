using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs
{
    public class SimCardPrimaryKeyDto
    {
        public string Iccid { get; set; } = null!;
        public string Imsi { get; set; } = null!;
        public string Msisdn { get; set; } = null!;
        public byte KIndId { get; set; }
    }
}
