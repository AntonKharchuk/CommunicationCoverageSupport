using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

namespace CommunicationCoverageSupport.BLL.Services.SimCards
{
    public interface ISimCardService
    {
        Task<IEnumerable<SimCardDto>> GetAllAsync();
        Task<SimCardDto?> GetByIccidAsync(string iccid);
        Task<SimCardDto?> GetByImsiAsync(string imsi);
        Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid);
        Task<SimCardFullInfoDto?> GetFullInfoByImsiAsync(string imsi);
        
        Task<(int StatusCode, string Message)> CreateAsync(SimCardDto dto);
        Task<(int StatusCode, string Message)> UpdateAsync(SimCardDto dto);
        Task<(int StatusCode, string Message)> UpdateInstalledStateAsync(SimCardPrimaryKeyDto keyDto, bool installed);
        Task<string> DrainAsync(string iccid, string imsi, string msisdn, int kIndId);
    }
}
