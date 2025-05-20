using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.Extensions.Configuration;

using MySqlConnector;

namespace CommunicationCoverageSupport.DAL.Repositories
{
    public class SimCardRepository : ISimCardRepository
    {
        private readonly string _connectionString;

        public SimCardRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<SimCardDto>> GetAllAsync()
        {
            var result = new List<SimCardDto>();
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT * FROM sim_cards", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapReader(reader));
            }

            return result;
        }

        public async Task<SimCardDto?> GetByIccidAsync(string iccid)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT * FROM sim_cards WHERE iccid = @iccid", conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);

            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapReader(reader) : null;
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

        public async Task<bool> CreateAsync(SimCardDto simCard)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(@"
                INSERT INTO sim_cards (
                    iccid, imsi, artworkId, accId, installed, purged,
                    ki1, cardOwnerId, pin1, pin2, puk1, puk2, adm1
                ) VALUES (
                    @iccid, @imsi, @artworkId, @accId, @installed, @purged,
                    @ki1, @cardOwnerId, @pin1, @pin2, @puk1, @puk2, @adm1
                )", conn);

            SetParameters(cmd, simCard);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateAsync(SimCardDto simCard)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(@"
                UPDATE sim_cards SET
                    imsi = @imsi,
                    artworkId = @artworkId,
                    accId = @accId,
                    installed = @installed,
                    purged = @purged,
                    ki1 = @ki1,
                    cardOwnerId = @cardOwnerId,
                    pin1 = @pin1,
                    pin2 = @pin2,
                    puk1 = @puk1,
                    puk2 = @puk2,
                    adm1 = @adm1
                WHERE iccid = @iccid
            ", conn);

            SetParameters(cmd, simCard);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string iccid)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("DELETE FROM sim_cards WHERE iccid = @iccid", conn);
            cmd.Parameters.AddWithValue("@iccid", iccid);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private void SetParameters(MySqlCommand cmd, SimCardDto dto)
        {
            cmd.Parameters.AddWithValue("@iccid", dto.Iccid);
            cmd.Parameters.AddWithValue("@imsi", dto.Imsi);
            cmd.Parameters.AddWithValue("@artworkId", dto.ArtworkId);
            cmd.Parameters.AddWithValue("@accId", dto.AccId);
            cmd.Parameters.AddWithValue("@installed", dto.Installed);
            cmd.Parameters.AddWithValue("@purged", dto.Purged);
            cmd.Parameters.AddWithValue("@ki1", dto.Ki1);
            cmd.Parameters.AddWithValue("@cardOwnerId", dto.CardOwnerId);
            cmd.Parameters.AddWithValue("@pin1", dto.Pin1);
            cmd.Parameters.AddWithValue("@pin2", dto.Pin2);
            cmd.Parameters.AddWithValue("@puk1", dto.Puk1);
            cmd.Parameters.AddWithValue("@puk2", dto.Puk2);
            cmd.Parameters.AddWithValue("@adm1", dto.Adm1);
        }

        private SimCardDto MapReader(MySqlDataReader reader)
        {
            return new SimCardDto
            {
                Iccid = reader.GetString("iccid"),
                Imsi = reader.GetString("imsi"),
                ArtworkId = reader.GetByte("artworkId"),
                AccId = reader.GetByte("accId"),
                Installed = reader.GetBoolean("installed"),
                Purged = reader.GetBoolean("purged"),
                Ki1 = reader.GetString("ki1"),
                CardOwnerId = reader.GetInt64("cardOwnerId"),
                Pin1 = reader.GetInt16("pin1"),
                Pin2 = reader.GetInt16("pin2"),
                Puk1 = reader.GetInt32("puk1"),
                Puk2 = reader.GetInt32("puk2"),
                Adm1 = reader.GetString("adm1")
            };
        }

        public Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid)
        {
            throw new NotImplementedException();
        }
    }
}
