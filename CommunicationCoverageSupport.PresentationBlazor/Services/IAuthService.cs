using CommunicationCoverageSupport.Models.DTOs.Auth;
namespace CommunicationCoverageSupport.PresentationBlazor.Services
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
    }

}
