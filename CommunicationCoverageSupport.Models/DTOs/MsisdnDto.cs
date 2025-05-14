using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs
{
    public class MsisdnDto
    {
        public string Number { get; set; } = string.Empty;
        public byte Class { get; set; }
        public bool Prop2 { get; set; }
    }
}
