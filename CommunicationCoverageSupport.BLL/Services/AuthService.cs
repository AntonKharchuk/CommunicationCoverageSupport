using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.DAL.Contexts;
using CommunicationCoverageSupport.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CommunicationCoverageSupport.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto registerDto)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyName == registerDto.CompanyName);

            if (company == null)
            {
                company = new Company
                {
                    CompanyName = registerDto.CompanyName
                };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
            }

            var user = new ApplicationUser
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CompanyId = company.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            var isAdmin = await IsAdminAsync(user.Id);
            var isSuperAdmin = await IsSuperAdminAsync(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("CompanyId", user.CompanyId.ToString()),
                new Claim(ClaimTypes.Role, isSuperAdmin ? "SuperAdmin" : isAdmin ? "Admin" : "User")
            };

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires.Value
            };
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            return await _context.ApplicationAdmins.AnyAsync(a => a.UserId == userId);
        }

        public async Task<bool> IsSuperAdminAsync(int userId)
        {
            var admin = await _context.ApplicationAdmins
                .Include(a => a.User)
                .ThenInclude(u => u.Company)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            return admin?.User.Company.CompanyName == "SuperAdminCompany";
        }
    }
}
