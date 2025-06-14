using Microsoft.EntityFrameworkCore;
using TicketService.Models;

namespace TicketService.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContext _context;

        public TicketRepository(TicketContext context)
        {
            _context = context;
        }
        public async Task<Ticket> ReserveTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> PurchaseTicket(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            ticket.IsPurchased = true;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelReservation(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Ticket>> GetAvailableTickets(int eventId)
        {
            return await _context.Tickets
                .Where(t => t.EventId == eventId && !t.IsPurchased)
                .ToListAsync();
        }
    }
}