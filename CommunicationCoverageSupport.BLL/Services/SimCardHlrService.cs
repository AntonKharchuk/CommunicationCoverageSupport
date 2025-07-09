using CommunicationCoverageSupport.DAL.Clients;
using CommunicationCoverageSupport.DAL.Clients.HLR;
using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.DAL.Repositories.SimCards;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

namespace CommunicationCoverageSupport.BLL.Services.SimCards
{
    public class SimCardHlrService : ISimCardService
    {
        private readonly ISimCardRepository _repository;
        private readonly ICai3gHlrClient _cai3gHlrClient;

        public SimCardHlrService(ISimCardRepository repository, ICai3gHlrClient cai3gHlrClient)
        {
            _repository = repository;
            _cai3gHlrClient = cai3gHlrClient;
        }

        public Task<IEnumerable<SimCardDto>> GetAllAsync() => _repository.GetAllAsync();
        public Task<SimCardDto?> GetByIccidAsync(string iccid) => _repository.GetByIccidAsync(iccid);
        public Task<SimCardDto?> GetByImsiAsync(string imsi) => _repository.GetByImsiAsync(imsi);
        public Task<SimCardFullInfoDto?> GetFullInfoByIccidAsync(string iccid) => _repository.GetFullInfoByIccidAsync(iccid);

        //public async Task<(int StatusCode, string Message)> CreateAsync(SimCardDto dto)
        //{
        //    string? hlrMessage = null;

        //    if (dto.Installed)
        //    {
        //        hlrMessage = await _sshHlrClient.AddSimCardAsync(dto.Imsi);

        //        if (!hlrMessage.Contains("IMSI added", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return (400, $"HLR Error: {hlrMessage}");
        //        }
        //    }
        //    hlrMessage = hlrMessage?.Trim() ?? "Skipped";

        //    var dbResult = await _repository.CreateAsync(dto);
        //    if (!dbResult)
        //    {
        //        return (500, $"HLR: {hlrMessage}, DB: Failed to add SIM card");
        //    }

        //    return (200, $"HLR: {hlrMessage}, DB: SIM card added successfully");
        //}

        //public async Task<(int StatusCode, string Message)> UpdateAsync(SimCardDto dto)
        //{
        //    string? hlrMessage = null;

        //    if (dto.Installed)
        //    {
        //        hlrMessage = await _sshHlrClient.AddSimCardAsync(dto.Imsi);

        //        if (!hlrMessage.Contains("IMSI added", StringComparison.OrdinalIgnoreCase) &&
        //        !hlrMessage.Contains("IMSI already exists", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return (400, $"HLR Error: {hlrMessage}");
        //        }
        //    }
        //    else
        //    {
        //        hlrMessage = await _sshHlrClient.RemoveSimCardAsync(dto.Imsi);
        //        if (!hlrMessage.Contains("IMSI removed", StringComparison.OrdinalIgnoreCase) &&
        //            !hlrMessage.Contains("IMSI not found", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return (400, $"HLR Error: {hlrMessage}");
        //        }
        //    }

        //    hlrMessage = hlrMessage?.Trim();

        //    var dbResult = await _repository.UpdateAsync(dto);
        //    if (!dbResult)
        //    {
        //        return (500, $"HLR: {hlrMessage}, DB: Failed to update SIM card");
        //    }

        //    return (200, $"HLR: {hlrMessage}, DB: SIM card updated successfully");
        //}

        //public async Task<(int StatusCode, string Message)> UpdateInstalledStateAsync(SimCardPrimaryKeyDto keyDto, bool installed)
        //{
        //    string? hlrMessage = null;

        //    if (installed)
        //    {
        //        hlrMessage = await _sshHlrClient.AddSimCardAsync(keyDto.Imsi);

        //        if (!hlrMessage.Contains("IMSI added", StringComparison.OrdinalIgnoreCase) &&
        //            !hlrMessage.Contains("IMSI already exists", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return (400, $"HLR Error: {hlrMessage}");
        //        }
        //    }
        //    else
        //    {
        //        hlrMessage = await _sshHlrClient.RemoveSimCardAsync(keyDto.Imsi);
        //        if (!hlrMessage.Contains("IMSI removed", StringComparison.OrdinalIgnoreCase) &&
        //            !hlrMessage.Contains("IMSI not found", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return (400, $"HLR Error: {hlrMessage}");
        //        }
        //    }

        //    hlrMessage = hlrMessage?.Trim();

        //    var dbResult = await _repository.UpdateInstalledStateAsync(keyDto, installed);
        //    if (!dbResult)
        //    {
        //        return (500, $"HLR: {hlrMessage}, DB: Failed to update Installed state");
        //    }

        //    return (200, $"HLR: {hlrMessage}, DB: Installed state updated successfully");
        //}



        public async Task<string> DrainAsync(string iccid, string imsi, string msisdn, int kIndId)
        {
            var simCard = await _repository.GetByIccidAsync(iccid);

            if (simCard == null)
            {
                return "Error: SIM card not found.";
            }

            if (simCard.Installed)
            {
                return "Error: Cannot drain an installed SIM card.";
            }

            return await _repository.DrainAsync(iccid, imsi, msisdn, kIndId);
        }

        public Task<(int StatusCode, string Message)> CreateAsync(SimCardDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<(int StatusCode, string Message)> UpdateAsync(SimCardDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<(int StatusCode, string Message)> UpdateInstalledStateAsync(SimCardPrimaryKeyDto keyDto, bool installed)
        {
            //await _cai3gHlrClient.LoginAsync();
            var msisdnState = await _cai3gHlrClient.GetAsync("255702000100000");

            //msisdnState

            //return (400, context);
            return (400, "test");
        }
    }
}
