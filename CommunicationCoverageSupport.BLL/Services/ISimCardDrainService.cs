using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public interface ISimCardDrainService
    {
        Task<IEnumerable<SimCardDrainDto>> GetAllAsync();
        Task<SimCardDrainDto?> GetByIccidAsync(string iccid);
        Task<SimCardDrainFullInfoDto?> GetFullInfoByIccidAsync(string iccid);
        Task<bool> DeleteAsync(string iccid, string imsi, string msisdn, byte kIndId);
    }
}
