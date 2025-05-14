using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class MsisdnService : IMsisdnService
    {
        private readonly IMsisdnRepository _repository;

        public MsisdnService(IMsisdnRepository repository)
        {
            _repository = repository;
        }

        public Task<List<MsisdnDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<MsisdnDto?> GetByMsisdnAsync(string msisdn)
        {
            return _repository.GetByMsisdnAsync(msisdn);
        }

        public Task<bool> CreateAsync(MsisdnDto dto)
        {
            return _repository.CreateAsync(dto);
        }

        public Task<bool> UpdateAsync(MsisdnDto dto)
        {
            return _repository.UpdateAsync(dto);
        }

        public Task<bool> DeleteAsync(string msisdn)
        {
            return _repository.DeleteAsync(msisdn);
        }
    }
}
