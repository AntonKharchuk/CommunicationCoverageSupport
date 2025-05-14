using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _repository;

        public SubscriberService(ISubscriberRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SubscriberDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<SubscriberDto?> GetByMsisdnAsync(string msisdn)
        {
            return _repository.GetByMsisdnAsync(msisdn);
        }

        public Task<bool> CreateAsync(SubscriberDto dto)
        {
            return _repository.CreateAsync(dto);
        }

        public Task<bool> DeleteAsync(string msisdn)
        {
            return _repository.DeleteAsync(msisdn);
        }
    }
}
