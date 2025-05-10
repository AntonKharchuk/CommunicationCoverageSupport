using CommunicationCoverageSupport.DAL.Contexts;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users.Include(u => u.Company).FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || user.PasswordHash != dto.Password) return null;

            var isAdmin = await _context.ApplicationAdmins.AnyAsync(a => a.UserId == user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto dto, bool isAdmin)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username)) return false;

            var user = new ApplicationUser
            {
                Username = dto.Username,
                PasswordHash = dto.Password,
                Company = new Company { CompanyName = dto.CompanyName }
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (isAdmin)
            {
                _context.ApplicationAdmins.Add(new ApplicationAdmin { UserId = user.Id });
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            return await _context.ApplicationAdmins.AnyAsync(a => a.UserId == userId);
        }
    }
}
