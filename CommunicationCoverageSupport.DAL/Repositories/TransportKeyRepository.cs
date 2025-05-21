using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.Extensions.Configuration;

using MySqlConnector;

namespace CommunicationCoverageSupport.DAL.Repositories.TransportKeys
{
    public class TransportKeyRepository : ITransportKeyRepository
    {
        private readonly string _connectionString;

        public TransportKeyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<TransportKeyDto>> GetAllAsync()
        {
            var list = new List<TransportKeyDto>();
            const string query = "SELECT id, kInd FROM transportKey";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new TransportKeyDto
                {
                    Id = reader.GetByte("id"),
                    KInd = reader.GetString("kInd")
                });
            }

            return list;
        }

        public async Task<TransportKeyDto?> GetByIdAsync(byte id)
        {
            const string query = "SELECT id, kInd FROM transportKey WHERE id = @id";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await using var reader = await cmd.ExecuteReaderAsync();

            return await reader.ReadAsync()
                ? new TransportKeyDto
                {
                    Id = reader.GetByte("id"),
                    KInd = reader.GetString("kInd")
                }
                : null;
        }

        public async Task<bool> CreateAsync(TransportKeyDto dto)
        {
            const string query = "INSERT INTO transportKey (id, kInd) VALUES (@id, @kInd)";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", dto.Id);
            cmd.Parameters.AddWithValue("@kInd", dto.KInd);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TransportKeyDto dto)
        {
            const string query = "UPDATE transportKey SET kInd = @kInd WHERE id = @id";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", dto.Id);
            cmd.Parameters.AddWithValue("@kInd", dto.KInd);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(byte id)
        {
            const string query = "DELETE FROM transportKey WHERE id = @id";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
