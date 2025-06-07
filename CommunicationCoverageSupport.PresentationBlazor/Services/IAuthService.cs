using CommunicationCoverageSupport.Models.DTOs.Auth;
namespace CommunicationCoverageSupport.PresentationBlazor.Services
{
    public interface IAuthService
    {
        Task<string?> RegisterUserAsync(UserRegisterDto dto);
        Task<string?> RegisterAdminAsync(UserRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
    }

}
