// File: CommunicationCoverageSupport/DAL/Repositories/Auth/AuthRepository.cs
using CommunicationCoverageSupport.Models.DTOs.Auth;
using CommunicationCoverageSupport.Models.Entities;

using Microsoft.Extensions.Configuration;

using MySqlConnector;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> RegisterAsync(UserRegister registeredUser)
        {
            var allowedRoles = new[] { "user", "admin" };
            if (!allowedRoles.Contains(registeredUser.Role.ToLower()))
                throw new ArgumentException("Invalid role");

            var hash = BCrypt.Net.BCrypt.HashPassword(registeredUser.Password);

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "INSERT INTO users (username, password_hash, role) VALUES (@username, @password, @role)",
                conn);
            cmd.Parameters.AddWithValue("@username", registeredUser.Username);
            cmd.Parameters.AddWithValue("@password", hash);
            cmd.Parameters.AddWithValue("@role", registeredUser.Role);

            try
            {
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return false;
            }
        }

        public async Task<(string Username, string Role)?> LoginAsync(UserLoginDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT username, password_hash, role FROM users WHERE username = @username",
                conn);
            cmd.Parameters.AddWithValue("@username", dto.Username);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            var username = reader.GetString("username");
            var hash = reader.GetString("password_hash");
            var role = reader.GetString("role");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, hash))
                return null;

            return (username, role);
        }
    }
}
