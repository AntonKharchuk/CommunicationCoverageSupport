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
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly string _connectionString;

        public SubscriberRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<SubscriberDto>> GetAllAsync()
        {
            var list = new List<SubscriberDto>();
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT msisdn, imsi FROM subscriber", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new SubscriberDto
                {
                    Msisdn = reader.GetString("msisdn"),
                    Imsi = reader.GetString("imsi")
                });
            }

            return list;
        }

        public async Task<SubscriberDto?> GetByMsisdnAsync(string msisdn)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT msisdn, imsi FROM subscriber WHERE msisdn = @msisdn", conn);
            cmd.Parameters.AddWithValue("@msisdn", msisdn);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new SubscriberDto
                {
                    Msisdn = reader.GetString("msisdn"),
                    Imsi = reader.GetString("imsi")
                };
            }

            return null;
        }

        public async Task<bool> CreateAsync(SubscriberDto dto)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("INSERT INTO subscriber (msisdn, imsi) VALUES (@msisdn, @imsi)", conn);
            cmd.Parameters.AddWithValue("@msisdn", dto.Msisdn);
            cmd.Parameters.AddWithValue("@imsi", dto.Imsi);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string msisdn)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("DELETE FROM subscriber WHERE msisdn = @msisdn", conn);
            cmd.Parameters.AddWithValue("@msisdn", msisdn);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }

}
