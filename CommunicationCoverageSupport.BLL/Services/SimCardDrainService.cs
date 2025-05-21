using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class SimCardDrainService : ISimCardDrainService
    {
        private readonly ISimCardDrainRepository _repository;

        public SimCardDrainService(ISimCardDrainRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SimCardDrainDto>> GetAllAsync() => _repository.GetAllAsync();
        public Task<SimCardDrainDto?> GetByIccidAsync(string iccid) => _repository.GetByIccidAsync(iccid);
        public Task<SimCardDrainFullInfoDto?> GetFullInfoByIccidAsync(string iccid) => _repository.GetFullInfoByIccidAsync(iccid);
        public Task<bool> DeleteAsync(string iccid, string imsi, string msisdn, byte kIndId) => _repository.DeleteAsync(iccid, imsi, msisdn, kIndId);
    }
}
