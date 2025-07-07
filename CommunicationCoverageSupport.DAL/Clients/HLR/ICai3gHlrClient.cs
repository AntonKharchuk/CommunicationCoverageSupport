using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Clients.HLR
{
    public interface ICai3gHlrClient
    {
        Task<string> GetAsync(string imsi);

        Task LoginAsync();
    }
}
