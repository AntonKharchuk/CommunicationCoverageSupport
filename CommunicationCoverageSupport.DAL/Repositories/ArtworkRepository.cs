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
    public class ArtworkRepository : IArtworkRepository
    {
        private readonly string _connectionString;

        public ArtworkRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<ArtworkDto>> GetAllAsync()
        {
            var list = new List<ArtworkDto>();
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT id, name FROM artwork", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ArtworkDto
                {
                    Id = reader.GetByte("id"),
                    Name = reader.GetString("name")
                });
            }

            return list;
        }

        public async Task<ArtworkDto?> GetByIdAsync(byte id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT id, name FROM artwork WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ArtworkDto
                {
                    Id = reader.GetByte("id"),
                    Name = reader.GetString("name")
                };
            }

            return null;
        }

        public async Task<bool> CreateAsync(string name)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("INSERT INTO artwork (name) VALUES (@name)", conn);
            cmd.Parameters.AddWithValue("@name", name);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateAsync(byte id, string name)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("UPDATE artwork SET name = @name WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(byte id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("DELETE FROM artwork WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
