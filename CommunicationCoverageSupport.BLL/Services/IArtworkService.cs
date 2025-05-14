using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public interface IArtworkService
    {
        Task<List<ArtworkDto>> GetAllAsync();
        Task<ArtworkDto?> GetByIdAsync(byte id);
        Task<bool> CreateAsync(string name);
        Task<bool> UpdateAsync(byte id, string name);
        Task<bool> DeleteAsync(byte id);
    }
}
