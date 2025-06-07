using System.Net.Http.Json;
using CommunicationCoverageSupport.Models.DTOs;

namespace CommunicationCoverageSupport.PresentationBlazor.Services.Acc
{
    public interface IAccService
    {
        Task<List<AccDto>> GetAllAsync();
        Task<AccDto?> GetByIdAsync(int id);
        Task CreateAsync(string name);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
    }
}
