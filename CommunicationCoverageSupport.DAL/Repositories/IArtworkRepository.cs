using CommunicationCoverageSupport.Models.DTOs;


namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface IArtworkRepository
    {
        Task<List<ArtworkDto>> GetAllAsync();
        Task<ArtworkDto?> GetByIdAsync(byte id);
        Task<bool> CreateAsync(string name);
        Task<bool> UpdateAsync(byte id, string name);
        Task<bool> DeleteAsync(byte id);
    }
}
