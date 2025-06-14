using TicketService.Models;

namespace TicketService.Repository
{
    public interface ITicketRepository
    {
        Task<Ticket> ReserveTicket(Ticket ticket);
        Task<bool> PurchaseTicket(int ticketId);
        Task<bool> CancelReservation(int ticketId);
        Task<IEnumerable<Ticket>> GetAvailableTickets(int eventId);

    }
}
