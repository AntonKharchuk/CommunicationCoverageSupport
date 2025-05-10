using CommunicationCoverageSupport.Models.DTOs;

namespace CommunicationCoverageSupport.BLL.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequestDto dto);
        Task<bool> RegisterAsync(RegisterRequestDto dto, bool isAdmin);
        Task<bool> IsAdminAsync(int userId);
    }
}
