using CommunicationCoverageSupport.Models.DTOs.Auth;
using CommunicationCoverageSupport.Models.Entities;

namespace CommunicationCoverageSupport.DAL.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<bool> RegisterAsync(UserRegister registeredUser);
        Task<(string Username, string Role)?> LoginAsync(UserLoginDto dto);
    }
}
