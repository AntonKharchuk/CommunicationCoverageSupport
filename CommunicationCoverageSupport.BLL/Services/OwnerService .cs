using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _repository;

        public OwnerService(IOwnerRepository repository)
        {
            _repository = repository;
        }

        public Task<List<OwnerDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<OwnerDto?> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> CreateAsync(OwnerDto dto)
        {
            return _repository.CreateAsync(dto);
        }

        public Task<bool> UpdateAsync(OwnerDto dto)
        {
            return _repository.UpdateAsync(dto);
        }

        public Task<bool> DeleteAsync(long id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
