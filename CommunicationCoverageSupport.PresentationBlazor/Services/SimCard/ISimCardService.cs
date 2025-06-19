using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;
using CommunicationCoverageSupport.PresentationBlazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.PresentationBlazor.Services.SimCard
{
    public interface ISimCardService
    {
        Task<ApiResponse<List<SimCardDto>>> GetAllAsync();
        Task<ApiResponse<SimCardDto>> GetByIccidAsync(string iccid);
        Task<ApiResponse<SimCardFullInfoDto>> GetFullByIccidAsync(string iccid);
        Task<ApiResponse<bool>> CreateAsync(SimCardDto dto);
        Task<ApiResponse<bool>> UpdateAsync(SimCardDto dto);
        Task<ApiResponse<bool>> UpdateInstalledAsync(UpdateInstalledRequestDto request);
        Task<ApiResponse<bool>> DrainAsync(SimCardPrimaryKeyDto request);
        Task<ApiResponse<List<SimCardDrainDto>>> GetDrainsAsync();
        Task<ApiResponse<SimCardDrainDto>> GetDrainByIccidAsync(string iccid);
        Task<ApiResponse<SimCardDrainFullInfoDto>> GetDrainFullByIccidAsync(string iccid);
        Task<ApiResponse<bool>> DeleteDrainAsync(SimCardPrimaryKeyDto request);
    }
}