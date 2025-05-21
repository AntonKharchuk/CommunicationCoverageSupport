using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface ISimCardDrainRepository
    {
        Task<IEnumerable<SimCardDrainDto>> GetAllAsync();
        Task<SimCardDrainDto?> GetByIccidAsync(string iccid);
        Task<bool> DeleteAsync(string iccid, string imsi, string msisdn, byte kIndId);
        Task<SimCardDrainFullInfoDto?> GetFullInfoByIccidAsync(string iccid);
    }
}
