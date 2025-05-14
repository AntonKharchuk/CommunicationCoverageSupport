using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

public interface IAvailabilityRepository
{
    Task<List<FreeImsiDto>> GetFreeImsisAsync();
    Task<List<FreeMsisdnDto>> GetFreeMsisdnsAsync();
}