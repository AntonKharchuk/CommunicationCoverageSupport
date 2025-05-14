using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs
{
    public class SubscriberDto
    {
        public string Msisdn { get; set; } = string.Empty;
        public string Imsi { get; set; } = string.Empty;
    }
}
