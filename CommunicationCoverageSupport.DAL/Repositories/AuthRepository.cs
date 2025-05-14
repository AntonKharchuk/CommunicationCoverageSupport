using CommunicationCoverageSupport.Models.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace CommunicationCoverageSupport.DAL.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public AuthRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
            _config = config;
        }

        public async Task<bool> RegisterAsync(UserRegisterDto dto)
        {
            //check that the role is valid
            var allowedRoles = new[] { "user", "admin" };
            if (!allowedRoles.Contains(dto.Role.ToLower()))
                throw new ArgumentException("Invalid role");

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("INSERT INTO users (username, password_hash, role) VALUES (@username, @password, @role)", conn);
            cmd.Parameters.AddWithValue("@username", dto.Username);
            cmd.Parameters.AddWithValue("@password", hash);
            cmd.Parameters.AddWithValue("@role", dto.Role);


            try
            {
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return false; // duplicate username
            }
        }


        public async Task<AuthResponseDto?> LoginAsync(UserLoginDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT id, username, password_hash, role FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", dto.Username);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            var id = reader.GetInt64("id");
            var username = reader.GetString("username");
            var hash = reader.GetString("password_hash");
            var role = reader.GetString("role");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, hash))
                return null;

            return GenerateJwt(id, username, role);
        }

        private AuthResponseDto GenerateJwt(long id, string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]!));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires!.Value
            };
        }
    }
}
