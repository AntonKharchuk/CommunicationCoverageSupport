using CommunicationCoverageSupport.Models.DTOs.SimCards;

namespace CommunicationCoverageSupport.BLL.Services.SimCards
{
    public interface ISimCardService
    {
        Task<List<SimCardDto>> GetAllAsync();                  // Admin only
        Task<List<SimCardDto>> GetAllByUserAsync(int userId);  // Regular user
        Task<SimCardDto?> GetByIccidAsync(string iccid);
        Task<bool> CreateAsync(CreateSimCardDto dto, int userId);
        Task<bool> DeleteAsync(string iccid, int userId);
    }

}
