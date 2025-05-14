using CommunicationCoverageSupport.Models.DTOs.Auth;

namespace CommunicationCoverageSupport.DAL.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<bool> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
    }
}
