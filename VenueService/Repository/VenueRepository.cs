using Microsoft.EntityFrameworkCore;
using VenueService.Models;
using VenueService.Repository;

namespace VenueService.Repository
{
    public class VenueRepository : IVenueRepository
    {
        private readonly VenueContext _context;

        public VenueRepository(VenueContext context)
        {
            _context = context;
        }

        public async Task<Venue> CreateVenue(Venue venue)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return venue;
        }

        public async Task<Venue> UpdateVenue(Venue venue)
        {
            var existingVenue = await _context.Venues.FindAsync(venue.Id);
            if (existingVenue == null) return null;

            existingVenue.Name = venue.Name;
            existingVenue.Location = venue.Location;
            existingVenue.Capacity = venue.Capacity;

            _context.Venues.Update(existingVenue);
            await _context.SaveChangesAsync();

            return existingVenue;
        }

        public async Task<Venue> GetVenueById(int venueId)
        {
            return await _context.Venues.FindAsync(venueId);
        }

        public async Task<IEnumerable<Venue>> GetAllVenues()
        {
            return await _context.Venues.ToListAsync();
        }
    }
}