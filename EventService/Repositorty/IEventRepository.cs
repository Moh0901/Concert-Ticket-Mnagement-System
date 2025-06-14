using EventService.Models;

namespace EventService.Repositorty
{
    public interface IEventRepository
    {
        Task<Event> CreateEvent(Event concertEvent);
        Task<Event> UpdateEvent(Event concertEvent);
        Task<IEnumerable<Event>> GetAllEvents();
        Task<Event> GetEventById(int eventId);

    }
}
