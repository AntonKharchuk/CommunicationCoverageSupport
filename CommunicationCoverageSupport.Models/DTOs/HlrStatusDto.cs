using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs
{
    public class HlrStatusDto
    {
        public string Imsi { get; set; } = null!;
        public string MsisdnState { get; set; } = string.Empty;
    }
}
