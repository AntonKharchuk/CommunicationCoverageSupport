using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

namespace CommunicationCoverageSupport.BLL.Services.SimCards
{
    public interface ISimCardService
    {
        Task<IEnumerable<SimCardDto>> GetAllAsync();
        Task<SimCardDto?> GetByIccidAsync(string iccid);
        Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid);
        Task<bool> CreateAsync(SimCardDto dto);
        Task<bool> UpdateAsync(SimCardDto dto);
        Task<bool> DeleteAsync(string iccid, string imsi, string msisdn, byte kIndId);
        Task<string> DrainAsync(string iccid, string imsi, string msisdn, byte kIndId);
    }
}
