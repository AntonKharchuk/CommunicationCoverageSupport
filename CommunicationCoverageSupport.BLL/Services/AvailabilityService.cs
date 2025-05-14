using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _repository;

        public AvailabilityService(IAvailabilityRepository repository)
        {
            _repository = repository;
        }

        public Task<List<FreeImsiDto>> GetFreeImsisAsync()
        {
            return _repository.GetFreeImsisAsync();
        }

        public Task<List<FreeMsisdnDto>> GetFreeMsisdnsAsync()
        {
            return _repository.GetFreeMsisdnsAsync();
        }
    }
}
