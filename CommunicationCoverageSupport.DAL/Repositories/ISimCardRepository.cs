using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface ISimCardRepository
    {
        Task<IEnumerable<SimCardDto>> GetAllAsync();
        Task<SimCardDto?> GetByIccidAsync(string iccid);
        Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid);
        Task<bool> CreateAsync(SimCardDto simCard);
        Task<bool> UpdateAsync(SimCardDto simCard);
        Task<string> DrainAsync(string iccid, string imsi, string msisdn, byte kIndId);
    }
}
