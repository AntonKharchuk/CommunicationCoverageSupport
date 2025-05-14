using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface IAccRepository
    {
        Task<List<AccDto>> GetAllAsync();
        Task<AccDto?> GetByIdAsync(byte id);
        Task<bool> CreateAsync(string name);
        Task<bool> UpdateAsync(byte id, string name);
        Task<bool> DeleteAsync(byte id);
    }
}
