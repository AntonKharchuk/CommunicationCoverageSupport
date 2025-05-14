using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface IMsisdnRepository
    {
        Task<List<MsisdnDto>> GetAllAsync();
        Task<MsisdnDto?> GetByMsisdnAsync(string msisdn);
        Task<bool> CreateAsync(MsisdnDto dto);
        Task<bool> UpdateAsync(MsisdnDto dto);
        Task<bool> DeleteAsync(string msisdn);
    }

}
