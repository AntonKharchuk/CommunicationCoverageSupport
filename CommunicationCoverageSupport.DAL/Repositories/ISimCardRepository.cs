using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface ISimCardRepository
    {
        Task<List<SimCardDto>> GetAllAsync();
        Task<SimCardDto?> GetByIccidAsync(string iccid);
        Task<bool> CreateAsync(SimCardDto simCard);
        Task<bool> UpdateAsync(SimCardDto simCard);
        Task<bool> DeleteAsync(string iccid);
    }
}
