using CommunicationCoverageSupport.DAL.Contexts;
using CommunicationCoverageSupport.Models.DTOs.SimCards;
using CommunicationCoverageSupport.Models.Entities;
using Microsoft.EntityFrameworkCore;
using CommunicationCoverageSupport.BLL.Services;

namespace CommunicationCoverageSupport.BLL.Services.SimCards
{
    public class SimCardService : ISimCardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public SimCardService(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<List<SimCardDto>> GetAllAsync()
        {
            return await _context.SimCards
                .Select(s => new SimCardDto
                {
                    Iccid = s.Iccid,
                    Imsi = s.Imsi,
                    Msisdn = s.Msisdn,
                    Produced = s.Produced,
                    Installed = s.Installed,
                    Purged = s.Purged,
                    Kl1 = s.Kl1,
                    Pin1 = s.Pin1,
                    Pin2 = s.Pin2,
                    Puk1 = s.Puk1,
                    Puk2 = s.Puk2,
                    Adm1 = s.Adm1,
                    ArtworkId = s.ArtworkId,
                    AccId = s.AccId,
                    CardOwnerId = s.CardOwnerId
                }).ToListAsync();
        }

        public async Task<List<SimCardDto>> GetAllByUserAsync(int userId)
        {
            return await _context.SimCards
                .Where(s => s.CardOwnerId == userId)
                .Select(s => new SimCardDto
                {
                    Iccid = s.Iccid,
                    Imsi = s.Imsi,
                    Msisdn = s.Msisdn,
                    Produced = s.Produced,
                    Installed = s.Installed,
                    Purged = s.Purged,
                    Kl1 = s.Kl1,
                    Pin1 = s.Pin1,
                    Pin2 = s.Pin2,
                    Puk1 = s.Puk1,
                    Puk2 = s.Puk2,
                    Adm1 = s.Adm1,
                    ArtworkId = s.ArtworkId,
                    AccId = s.AccId,
                    CardOwnerId = s.CardOwnerId
                }).ToListAsync();
        }

        public async Task<SimCardDto?> GetByIccidAsync(string iccid)
        {
            var s = await _context.SimCards.FirstOrDefaultAsync(s => s.Iccid == iccid);
            if (s == null) return null;

            return new SimCardDto
            {
                Iccid = s.Iccid,
                Imsi = s.Imsi,
                Msisdn = s.Msisdn,
                Produced = s.Produced,
                Installed = s.Installed,
                Purged = s.Purged,
                Kl1 = s.Kl1,
                Pin1 = s.Pin1,
                Pin2 = s.Pin2,
                Puk1 = s.Puk1,
                Puk2 = s.Puk2,
                Adm1 = s.Adm1,
                ArtworkId = s.ArtworkId,
                AccId = s.AccId,
                CardOwnerId = s.CardOwnerId
            };
        }

        public async Task<bool> CreateAsync(CreateSimCardDto dto, int userId)
        {
            if (!await _authService.IsAdminAsync(userId))
                return false;

            if (dto.Installed && dto.Purged)
                return false;

            if (await _context.SimCards.AnyAsync(x => x.Iccid == dto.Iccid))
                return false;

            var simCard = new SimCard
            {
                Iccid = dto.Iccid,
                Imsi = dto.Imsi,
                Msisdn = dto.Msisdn,
                Produced = dto.Produced,
                Installed = dto.Installed,
                Purged = dto.Purged,
                Kl1 = dto.Kl1,
                Pin1 = dto.Pin1,
                Pin2 = dto.Pin2,
                Puk1 = dto.Puk1,
                Puk2 = dto.Puk2,
                Adm1 = dto.Adm1,
                ArtworkId = dto.ArtworkId,
                AccId = dto.AccId,
                CardOwnerId = userId
            };

            _context.SimCards.Add(simCard);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string iccid, int userId)
        {
            if (!await _authService.IsAdminAsync(userId))
                return false;

            var card = await _context.SimCards.FirstOrDefaultAsync(s => s.Iccid == iccid);
            if (card == null) return false;

            _context.SimCards.Remove(card);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
