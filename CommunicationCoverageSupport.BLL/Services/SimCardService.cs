using CommunicationCoverageSupport.DAL.Clients;
using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.DAL.Repositories.SimCards;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

namespace CommunicationCoverageSupport.BLL.Services.SimCards
{
    public class SimCardService : ISimCardService
    {
        private readonly ISimCardRepository _repository;
        private readonly ISshHlrClient _sshHlrClient;

        public SimCardService(ISimCardRepository repository, ISshHlrClient sshHlrClient)
        {
            _repository = repository;
            _sshHlrClient = sshHlrClient;
        }

        public Task<IEnumerable<SimCardDto>> GetAllAsync() => _repository.GetAllAsync();
        public Task<SimCardDto?> GetByIccidAsync(string iccid) => _repository.GetByIccidAsync(iccid);
        public Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid) => _repository.GetFullInfoByIccidAsync(iccid);
        public Task<bool> CreateAsync(SimCardDto dto) => _repository.CreateAsync(dto);
        public Task<bool> UpdateAsync(SimCardDto dto) => _repository.UpdateAsync(dto);
        public Task<string> DrainAsync(string iccid, string imsi, string msisdn, int kIndId) => _repository.DrainAsync(iccid, imsi, msisdn, kIndId);

        public async Task<string> TestConnectionAsync()
        {
            var output = await _sshHlrClient.ExecuteCommandAsync("echo 'Hello from SSH!'");
            return output;
        }
    }
}
