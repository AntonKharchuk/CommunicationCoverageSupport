using CommunicationCoverageSupport.Models.DTOs.Auth;
using CommunicationCoverageSupport.DAL.Repositories.Auth;

namespace CommunicationCoverageSupport.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;

        public AuthService(IAuthRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> RegisterAsync(UserRegisterDto dto)
        {
            return _repository.RegisterAsync(dto);
        }

        public Task<AuthResponseDto?> LoginAsync(UserLoginDto dto)
        {
            return _repository.LoginAsync(dto);
        }
    }
}
