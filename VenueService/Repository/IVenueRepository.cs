using VenueService.Models;

namespace VenueService.Repository
{
    public interface IVenueRepository
    {
        Task<Venue> CreateVenue(Venue venue);
        Task<Venue> UpdateVenue(Venue venue);
        Task<Venue> GetVenueById(int venueId);
        Task<IEnumerable<Venue>> GetAllVenues();

    }
}
