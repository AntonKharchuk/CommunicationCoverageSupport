using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.Extensions.Configuration;

using MySqlConnector;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly string _connectionString;

    public AvailabilityRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<FreeImsiDto>> GetFreeImsisAsync()
    {
        var list = new List<FreeImsiDto>();
        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new MySqlCommand("SELECT imsi FROM imsi_free", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new FreeImsiDto { Imsi = reader.GetString("imsi") });
        }

        return list;
    }

    public async Task<List<FreeMsisdnDto>> GetFreeMsisdnsAsync()
    {
        var list = new List<FreeMsisdnDto>();
        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new MySqlCommand("SELECT msisdn FROM msisdn_free", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new FreeMsisdnDto { Msisdn = reader.GetString("msisdn") });
        }

        return list;
    }
}
