using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _repository;

        public ArtworkService(IArtworkRepository repository)
        {
            _repository = repository;
        }

        public Task<List<ArtworkDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<ArtworkDto?> GetByIdAsync(byte id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> CreateAsync(string name)
        {
            return _repository.CreateAsync(name);
        }

        public Task<bool> UpdateAsync(byte id, string name)
        {
            return _repository.UpdateAsync(id, name);
        }

        public Task<bool> DeleteAsync(byte id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
