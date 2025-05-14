using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class AuthService : IAuthService
    {
        public Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(RegisterRequestDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
