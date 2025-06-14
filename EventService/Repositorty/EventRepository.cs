using EventService.Models;
using EventService.Repositorty;
using Microsoft.EntityFrameworkCore;

namespace EventService.Repositorty
{
    public class EventRepository: IEventRepository
    {
        private readonly EventContext _context;

        public EventRepository(EventContext context)
        {
            _context = context;
        }
        public async Task<Event> CreateEvent(Event concertEvent)
        {
            _context.Events.Add(concertEvent);
            await _context.SaveChangesAsync();
            return concertEvent;
        }
        public async Task<Event> UpdateEvent(Event concertEvent)
        {
            var existingEvent = await _context.Events.FindAsync(concertEvent.Id);
            if (existingEvent == null) return null;

            existingEvent.Name = concertEvent.Name;
            existingEvent.Venue = concertEvent.Venue;
            existingEvent.Date = concertEvent.Date;
            existingEvent.Description = concertEvent.Description;
            existingEvent.Capacity = concertEvent.Capacity;
            existingEvent.TicketTypes = concertEvent.TicketTypes;

            _context.Events.Update(existingEvent);
            await _context.SaveChangesAsync();

            return existingEvent;
        }
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _context.Events.Include(e => e.TicketTypes).ToListAsync();
        }
        public async Task<Event> GetEventById(int eventId)
        {
            return await _context.Events.Include(e => e.TicketTypes)
                                        .FirstOrDefaultAsync(e => e.Id == eventId);
        }
    }
}