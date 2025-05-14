using CommunicationCoverageSupport.Models.DTOs;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly string _connectionString;

        public OwnerRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<OwnerDto>> GetAllAsync()
        {
            var list = new List<OwnerDto>();
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT id, name FROM owners", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new OwnerDto
                {
                    Id = reader.GetInt64("id"),
                    Name = reader.GetString("name")
                });
            }

            return list;
        }

        public async Task<OwnerDto?> GetByIdAsync(long id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT id, name FROM owners WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new OwnerDto
                {
                    Id = reader.GetInt64("id"),
                    Name = reader.GetString("name")
                };
            }

            return null;
        }

        public async Task<bool> CreateAsync(OwnerDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("INSERT INTO owners (id, name) VALUES (@id, @name)", conn);
            cmd.Parameters.AddWithValue("@id", dto.Id);
            cmd.Parameters.AddWithValue("@name", dto.Name);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateAsync(OwnerDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("UPDATE owners SET name = @name WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", dto.Id);
            cmd.Parameters.AddWithValue("@name", dto.Name);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("DELETE FROM owners WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
