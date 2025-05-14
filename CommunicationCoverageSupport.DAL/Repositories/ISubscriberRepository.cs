using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public interface ISubscriberRepository
    {
        Task<List<SubscriberDto>> GetAllAsync();
        Task<SubscriberDto?> GetByMsisdnAsync(string msisdn);
        Task<bool> CreateAsync(SubscriberDto dto);
        Task<bool> DeleteAsync(string msisdn);
    }
}
