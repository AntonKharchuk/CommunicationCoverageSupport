using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Clients
{
    public interface ISshHlrClient
    {
        public Task<string> ExecuteCommandAsync(string command);
    }
}
