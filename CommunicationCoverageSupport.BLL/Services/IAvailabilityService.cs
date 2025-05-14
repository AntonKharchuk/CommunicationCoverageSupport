using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

namespace CommunicationCoverageSupport.BLL.Services
{
    public interface IAvailabilityService
    {
        Task<List<FreeImsiDto>> GetFreeImsisAsync();
        Task<List<FreeMsisdnDto>> GetFreeMsisdnsAsync();
    }
}
