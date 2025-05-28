using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.DTOs
{
    public class UpdateInstalledRequestDto
    {
        public SimCardPrimaryKeyDto PrimaryKey { get; set; } = null!;
        public bool Installed { get; set; }

    }
}
