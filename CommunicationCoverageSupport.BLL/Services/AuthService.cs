using CommunicationCoverageSupport.Models.DTOs.Auth;
using CommunicationCoverageSupport.DAL.Repositories.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CommunicationCoverageSupport.Models.Entities;

namespace CommunicationCoverageSupport.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public Task<bool> RegisterAsync(UserRegister registeredUser)
        {
            return _repository.RegisterAsync(registeredUser);
        }

        public async Task<AuthResponseDto?> LoginAsync(UserLoginDto dto)
        {
            var userInfo = await _repository.LoginAsync(dto);
            if (userInfo == null)
                return null;

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Value.Username),
                new Claim(ClaimTypes.Name, userInfo.Value.Username),
                new Claim(ClaimTypes.Role, userInfo.Value.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                Expiration = token.ValidTo
            };
        }
    }
}
