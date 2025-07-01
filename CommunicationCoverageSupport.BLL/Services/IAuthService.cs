using CommunicationCoverageSupport.Models.DTOs.Auth;
using CommunicationCoverageSupport.Models.Entities;

namespace CommunicationCoverageSupport.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRegister registeredUser);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
        Task<AuthResponseDto?> TestLoginAsync();

    }
}
