using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class SimCardService : ISimCardService
    {
        private readonly ISimCardRepository _repository;

        public SimCardService(ISimCardRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SimCardDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<SimCardDto?> GetByIccidAsync(string iccid)
        {
            return _repository.GetByIccidAsync(iccid);
        }
        public Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid)
        {
            return _repository.GetFullInfoByIccidAsync(iccid);
        }

        public Task<bool> CreateAsync(SimCardDto simCard)
        {
            return _repository.CreateAsync(simCard);
        }

        public Task<bool> UpdateAsync(SimCardDto simCard)
        {
            return _repository.UpdateAsync(simCard);
        }

        public Task<bool> DeleteAsync(string iccid)
        {
            return _repository.DeleteAsync(iccid);
        }
    }
}
