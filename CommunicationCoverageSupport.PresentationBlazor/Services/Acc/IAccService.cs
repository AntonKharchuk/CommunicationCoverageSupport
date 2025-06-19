using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.PresentationBlazor.Models;
using System.Net.Http.Json;

namespace CommunicationCoverageSupport.PresentationBlazor.Services.Acc
{
    public interface IAccService
    {
        Task<ApiResponse<List<AccDto>>> GetAllAsync();
        Task<ApiResponse<AccDto>> GetByIdAsync(int id);
        Task<ApiResponse<bool>> CreateAsync(string name);
        Task<ApiResponse<bool>> UpdateAsync(int id, string name);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
