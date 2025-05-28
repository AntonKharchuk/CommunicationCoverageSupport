using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.Extensions.Configuration;

using MySqlConnector;

namespace CommunicationCoverageSupport.DAL.Repositories.SimCards
{
    public class SimCardRepository : ISimCardRepository
    {
        private readonly string _connectionString;

        public SimCardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<SimCardDto>> GetAllAsync()
        {
            var list = new List<SimCardDto>();
            const string query = "SELECT * FROM simCards";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                list.Add(MapToSimCard(reader));

            return list;
        }

        public async Task<SimCardDto?> GetByIccidAsync(string iccid)
        {
            const string query = "SELECT * FROM simCards WHERE iccid = @iccid";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);

            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToSimCard(reader) : null;
        }

        public async Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid)
        {
            const string query = @"
SELECT s.*, a.name AS accName, ar.name AS artworkName, o.name AS ownerName, tk.kInd
FROM simCards s
JOIN acc a ON s.accId = a.id
JOIN artwork ar ON s.artworkId = ar.id
JOIN owners o ON s.cardOwnerId = o.id
JOIN transportKey tk ON s.kIndId = tk.id
WHERE s.iccid = @iccid";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new SimCardFullInfoDto
            {
                SimCard = MapToSimCard(reader),
                Acc = new AccDto { Id = reader.GetByte("accId"), Name = reader.GetString("accName") },
                Artwork = new ArtworkDto { Id = reader.GetByte("artworkId"), Name = reader.GetString("artworkName") },
                Owner = new OwnerDto { Id = reader.GetInt64("cardOwnerId"), Name = reader.GetString("ownerName") },
                TransportKey = new TransportKeyDto { Id = reader.GetByte("kIndId"), KInd = reader.GetString("kInd") }
            };
        }

        public async Task<bool> CreateAsync(SimCardDto dto)
        {
            const string query = @"
INSERT INTO simCards (iccid, imsi, msisdn, kIndId, ki, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId)
VALUES (@iccid, @imsi, @msisdn, @kIndId, @ki, @pin1, @pin2, @puk1, @puk2, @adm1, @artworkId, @accId, @installed, @cardOwnerId)";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            SetSimCardParams(cmd, dto);

            try
            {
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(SimCardDto dto)
        {
            const string query = @"
UPDATE simCards SET
    kIndId = @kIndId,
    ki = @ki,
    pin1 = @pin1,
    pin2 = @pin2,
    puk1 = @puk1,
    puk2 = @puk2,
    adm1 = @adm1,
    artworkId = @artworkId,
    accId = @accId,
    installed = @installed,
    cardOwnerId = @cardOwnerId
WHERE iccid = @iccid AND imsi = @imsi AND msisdn = @msisdn AND kIndId = @kIndId";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);
            SetSimCardParams(cmd, dto);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateInstalledStateAsync(SimCardPrimaryKeyDto keyDto, bool installed)
        {
            const string query = @"
UPDATE simCards SET
    installed = @installed
WHERE iccid = @iccid AND imsi = @imsi AND msisdn = @msisdn AND kIndId = @kIndId";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@installed", installed);
            cmd.Parameters.AddWithValue("@iccid", keyDto.Iccid);
            cmd.Parameters.AddWithValue("@imsi", keyDto.Imsi);
            cmd.Parameters.AddWithValue("@msisdn", keyDto.Msisdn);
            cmd.Parameters.AddWithValue("@kIndId", keyDto.KIndId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }


        public async Task<string> DrainAsync(string iccid, string imsi, string msisdn, int kIndId)
        {
            const string query = "CALL drainOneSim(@iccid, @imsi, @msisdn, @kIndId)";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@iccid", iccid);
            cmd.Parameters.AddWithValue("@imsi", imsi);
            cmd.Parameters.AddWithValue("@msisdn", msisdn);
            cmd.Parameters.AddWithValue("@kIndId", kIndId);

            try
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                return await reader.ReadAsync() ? reader.GetString("status_message") : "Unknown status";
            }
            catch (MySqlException ex)
            {
                return ex.Message;
            }
        }

        private static SimCardDto MapToSimCard(MySqlDataReader reader) => new SimCardDto
        {
            Iccid = reader.GetString("iccid"),
            Imsi = reader.GetString("imsi"),
            Msisdn = reader.GetString("msisdn"),
            KIndId = reader.GetInt32("kIndId"),
            Ki = reader.GetString("ki"),
            Pin1 = reader.GetInt16("pin1"),
            Pin2 = reader.GetInt16("pin2"),
            Puk1 = reader.GetInt32("puk1"),
            Puk2 = reader.GetInt32("puk2"),
            Adm1 = reader.GetString("adm1"),
            ArtworkId = reader.GetByte("artworkId"),
            AccId = reader.GetByte("accId"),
            Installed = reader.GetBoolean("installed"),
            CardOwnerId = reader.GetInt64("cardOwnerId")
        };

        private static void SetSimCardParams(MySqlCommand cmd, SimCardDto dto)
        {
            cmd.Parameters.AddWithValue("@iccid", dto.Iccid);
            cmd.Parameters.AddWithValue("@imsi", dto.Imsi);
            cmd.Parameters.AddWithValue("@msisdn", dto.Msisdn);
            cmd.Parameters.AddWithValue("@kIndId", dto.KIndId);
            cmd.Parameters.AddWithValue("@ki", dto.Ki);
            cmd.Parameters.AddWithValue("@pin1", dto.Pin1);
            cmd.Parameters.AddWithValue("@pin2", dto.Pin2);
            cmd.Parameters.AddWithValue("@puk1", dto.Puk1);
            cmd.Parameters.AddWithValue("@puk2", dto.Puk2);
            cmd.Parameters.AddWithValue("@adm1", dto.Adm1);
            cmd.Parameters.AddWithValue("@artworkId", dto.ArtworkId);
            cmd.Parameters.AddWithValue("@accId", dto.AccId);
            cmd.Parameters.AddWithValue("@installed", dto.Installed);
            cmd.Parameters.AddWithValue("@cardOwnerId", dto.CardOwnerId);
        }
    }
}



//public async Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid)
//{
//    await using var conn = new MySqlConnection(_connectionString);
//    await conn.OpenAsync();

//    var query = @"
//        SELECT s.*, a.name AS artworkName, acc.name AS accName, o.name AS ownerName,
//               m.msisdn, m.class, m.prop2
//        FROM sim_cards s
//        JOIN artwork a ON s.artworkId = a.id
//        JOIN acc acc ON s.accId = acc.id
//        JOIN owners o ON s.cardOwnerId = o.id
//        LEFT JOIN subscriber sub ON s.imsi = sub.imsi
//        LEFT JOIN msisdn m ON sub.msisdn = m.msisdn
//        WHERE s.iccid = @iccid
//    ";

//    await using var cmd = new MySqlCommand(query, conn);
//    cmd.Parameters.AddWithValue("@iccid", iccid);
//    await using var reader = await cmd.ExecuteReaderAsync();

//    if (await reader.ReadAsync())
//    {
//        return new SimCardFullInfoDto
//        {
//            SimCard = new SimCardDto
//            {
//                Iccid = reader.GetString("iccid"),
//                Imsi = reader.GetString("imsi"),
//                ArtworkId = reader.GetByte("artworkId"),
//                AccId = reader.GetByte("accId"),
//                Installed = reader.GetBoolean("installed"),
//                Purged = reader.GetBoolean("purged"),
//                Ki1 = reader.GetString("ki1"),
//                CardOwnerId = reader.GetInt64("cardOwnerId"),
//                Pin1 = reader.GetInt16("pin1"),
//                Pin2 = reader.GetInt16("pin2"),
//                Puk1 = reader.GetInt32("puk1"),
//                Puk2 = reader.GetInt32("puk2"),
//                Adm1 = reader.GetString("adm1")
//            },
//            Artwork = new ArtworkDto
//            {
//                Id = reader.GetByte("artworkId"),
//                Name = reader.GetString("artworkName")
//            },
//            Acc = new AccDto
//            {
//                Id = reader.GetByte("accId"),
//                Name = reader.GetString("accName")
//            },
//            Owner = new OwnerDto
//            {
//                Id = reader.GetInt64("cardOwnerId"),
//                Name = reader.GetString("ownerName")
//            },
//            Msisdn = reader.IsDBNull(reader.GetOrdinal("msisdn")) ? null : new MsisdnDto
//            {
//                Msisdn = reader.GetString("msisdn"),
//                Class = reader.GetByte("class"),
//                Prop2 = reader.GetBoolean("prop2")
//            }
//        };
//    }

//    return null;
//}