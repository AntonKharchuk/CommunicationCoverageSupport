
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public class SimCardDrainRepository : ISimCardDrainRepository
    {
        private readonly string _connectionString;

        public SimCardDrainRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<SimCardDrainDto>> GetAllAsync()
        {
            var list = new List<SimCardDrainDto>();
            const string query = "SELECT * FROM simCardsDrain";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new SimCardDrainDto
                {
                    Iccid = reader.GetString("iccid"),
                    Imsi = reader.GetString("imsi"),
                    Msisdn = reader.GetString("msisdn"),
                    KIndId = reader.GetByte("kIndId"),
                    Ki1 = reader.GetString("ki1"),
                    Pin1 = reader.GetInt16("pin1"),
                    Pin2 = reader.GetInt16("pin2"),
                    Puk1 = reader.GetInt32("puk1"),
                    Puk2 = reader.GetInt32("puk2"),
                    Adm1 = reader.GetString("adm1"),
                    ArtworkId = reader.GetByte("artworkId"),
                    AccId = reader.GetByte("accId"),
                    Installed = reader.GetBoolean("installed"),
                    CardOwnerId = reader.GetInt64("cardOwnerId"),
                    CreateTimestamp = reader.GetDateTime("createTimestamp")
                });
            }

            return list;
        }

        public async Task<SimCardDrainDto?> GetByIccidAsync(string iccid)
        {
            const string query = "SELECT * FROM simCardsDrain WHERE iccid = @iccid";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new SimCardDrainDto
                {
                    Iccid = reader.GetString("iccid"),
                    Imsi = reader.GetString("imsi"),
                    Msisdn = reader.GetString("msisdn"),
                    KIndId = reader.GetByte("kIndId"),
                    Ki1 = reader.GetString("ki1"),
                    Pin1 = reader.GetInt16("pin1"),
                    Pin2 = reader.GetInt16("pin2"),
                    Puk1 = reader.GetInt32("puk1"),
                    Puk2 = reader.GetInt32("puk2"),
                    Adm1 = reader.GetString("adm1"),
                    ArtworkId = reader.GetByte("artworkId"),
                    AccId = reader.GetByte("accId"),
                    Installed = reader.GetBoolean("installed"),
                    CardOwnerId = reader.GetInt64("cardOwnerId"),
                    CreateTimestamp = reader.GetDateTime("createTimestamp")
                };
            }

            return null;
        }
        public async Task<SimCardDrainFullInfoDto?> GetFullInfoByIccidAsync(string iccid)
        {
            const string query = @"
    SELECT sd.*, 
           a.name AS artworkName, 
           acc.name AS accName, 
           t.kInd AS tkInd, 
           o.name AS ownerName
    FROM simCardsDrain sd
    JOIN artwork a ON a.id = sd.artworkId
    JOIN acc acc ON acc.id = sd.accId
    JOIN owners o ON o.id = sd.cardOwnerId
    JOIN transportKey t ON t.id = sd.kIndId
    WHERE sd.iccid = @iccid";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new SimCardDrainFullInfoDto
            {
                SimCardDrain = new SimCardDrainDto
                {
                    Iccid = reader.GetString("iccid"),
                    Imsi = reader.GetString("imsi"),
                    Msisdn = reader.GetString("msisdn"),
                    KIndId = reader.GetByte("kIndId"),
                    Ki1 = reader.GetString("ki1"),
                    Pin1 = reader.GetInt16("pin1"),
                    Pin2 = reader.GetInt16("pin2"),
                    Puk1 = reader.GetInt32("puk1"),
                    Puk2 = reader.GetInt32("puk2"),
                    Adm1 = reader.GetString("adm1"),
                    ArtworkId = reader.GetByte("artworkId"),
                    AccId = reader.GetByte("accId"),
                    Installed = reader.GetBoolean("installed"),
                    CardOwnerId = reader.GetInt64("cardOwnerId"),
                    CreateTimestamp = reader.GetDateTime("createTimestamp")
                },
                TransportKey = new TransportKeyDto
                {
                    Id = reader.GetByte("kIndId"),
                    KInd = reader.GetString("tkInd")
                },
                

                Artwork = new ArtworkDto
                {
                    Id = reader.GetByte("artworkId"),
                    Name = reader.GetString("artworkName")
                },
                Acc = new AccDto
                {
                    Id = reader.GetByte("accId"),
                    Name = reader.GetString("accName")
                },
                Owner = new OwnerDto
                {
                    Id = reader.GetInt64("cardOwnerId"),
                    Name = reader.GetString("ownerName")
                },
            };
        }


        public async Task<bool> DeleteAsync(string iccid, string imsi, string msisdn, byte kIndId)
        {
            const string query = @"
                DELETE FROM simCardsDrain 
                WHERE iccid = @iccid AND imsi = @imsi AND msisdn = @msisdn AND kIndId = @kIndId";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);
            cmd.Parameters.AddWithValue("@imsi", imsi);
            cmd.Parameters.AddWithValue("@msisdn", msisdn);
            cmd.Parameters.AddWithValue("@kIndId", kIndId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
