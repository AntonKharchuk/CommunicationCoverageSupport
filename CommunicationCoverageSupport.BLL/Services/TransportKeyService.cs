using CommunicationCoverageSupport.DAL.Repositories.TransportKeys;
using CommunicationCoverageSupport.Models.DTOs;

namespace CommunicationCoverageSupport.BLL.Services.TransportKeys
{
    public class TransportKeyService : ITransportKeyService
    {
        private readonly ITransportKeyRepository _repository;

        public TransportKeyService(ITransportKeyRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TransportKeyDto>> GetAllAsync() => _repository.GetAllAsync();
        public Task<TransportKeyDto?> GetByIdAsync(byte id) => _repository.GetByIdAsync(id);
        public Task<bool> CreateAsync(TransportKeyDto dto) => _repository.CreateAsync(dto);
        public Task<bool> UpdateAsync(TransportKeyDto dto) => _repository.UpdateAsync(dto);
        public Task<bool> DeleteAsync(byte id) => _repository.DeleteAsync(id);
    }
}
