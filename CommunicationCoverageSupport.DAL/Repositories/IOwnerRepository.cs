using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface IOwnerRepository
    {
        Task<List<OwnerDto>> GetAllAsync();
        Task<OwnerDto?> GetByIdAsync(long id);
        Task<bool> CreateAsync(OwnerDto dto);
        Task<bool> UpdateAsync(OwnerDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
