using CommunicationCoverageSupport.Models.DTOs;

namespace CommunicationCoverageSupport.BLL.Services.TransportKeys
{
    public interface ITransportKeyService
    {
        Task<IEnumerable<TransportKeyDto>> GetAllAsync();
        Task<TransportKeyDto?> GetByIdAsync(byte id);
        Task<bool> CreateAsync(TransportKeyDto dto);
        Task<bool> UpdateAsync(TransportKeyDto dto);
        Task<bool> DeleteAsync(byte id);
    }
}
