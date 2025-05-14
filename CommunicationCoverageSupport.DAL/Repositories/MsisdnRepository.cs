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
    public class MsisdnRepository : IMsisdnRepository
    {
        private readonly string _connectionString;

        public MsisdnRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<MsisdnDto>> GetAllAsync()
        {
            var list = new List<MsisdnDto>();
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT msisdn, class, prop2 FROM msisdn", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new MsisdnDto
                {
                    Msisdn = reader.GetString("msisdn"),
                    Class = reader.GetByte("class"),
                    Prop2 = reader.GetBoolean("prop2")
                });
            }

            return list;
        }

        public async Task<MsisdnDto?> GetByMsisdnAsync(string msisdn)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT msisdn, class, prop2 FROM msisdn WHERE msisdn = @msisdn", conn);
            cmd.Parameters.AddWithValue("@msisdn", msisdn);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new MsisdnDto
                {
                    Msisdn = reader.GetString("msisdn"),
                    Class = reader.GetByte("class"),
                    Prop2 = reader.GetBoolean("prop2")
                };
            }

            return null;
        }

        public async Task<bool> CreateAsync(MsisdnDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("INSERT INTO msisdn (msisdn, class, prop2) VALUES (@msisdn, @class, @prop2)", conn);
            cmd.Parameters.AddWithValue("@msisdn", dto.Msisdn);
            cmd.Parameters.AddWithValue("@class", dto.Class);
            cmd.Parameters.AddWithValue("@prop2", dto.Prop2);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateAsync(MsisdnDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("UPDATE msisdn SET class = @class, prop2 = @prop2 WHERE msisdn = @msisdn", conn);
            cmd.Parameters.AddWithValue("@msisdn", dto.Msisdn);
            cmd.Parameters.AddWithValue("@class", dto.Class);
            cmd.Parameters.AddWithValue("@prop2", dto.Prop2);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string msisdn)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("DELETE FROM msisdn WHERE msisdn = @msisdn", conn);
            cmd.Parameters.AddWithValue("@msisdn", msisdn);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
