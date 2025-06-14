using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketService.Models;
using TicketService.Repository;

namespace TicketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveTicket([FromBody] Ticket ticket)
        {
            var result = await _ticketRepository.ReserveTicket(ticket);
            return Ok(result);
        }

        [HttpPost("purchase/{ticketId}")]
        public async Task<IActionResult> PurchaseTicket(int ticketId)
        {
            var result = await _ticketRepository.PurchaseTicket(ticketId);
            return result ? Ok("Ticket purchased successfully") : BadRequest("Ticket not found");
        }

        [HttpDelete("cancel/{ticketId}")]
        public async Task<IActionResult> CancelReservation(int ticketId)
        {
            var result = await _ticketRepository.CancelReservation(ticketId);
            return result ? Ok("Reservation canceled successfully") : BadRequest("Ticket not found");
        }

        [HttpGet("availability/{eventId}")]
        public async Task<IActionResult> GetAvailableTickets(int eventId)
        {
            var result = await _ticketRepository.GetAvailableTickets(eventId);
            return Ok(result);
        }
    }
}


[Route("api/tickets")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;

    public TicketController(ITicketRepository ticketRepository, RabbitMqService rabbitMqService)
    {
        _ticketRepository = ticketRepository;
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost("updateAvailability")]
    public async Task<IActionResult> UpdateTicketAvailability(int eventId, int ticketId, bool isReserved, bool isPurchased)
    {
        var result = await _ticketRepository.UpdateTicketAvailability(eventId, ticketId, isReserved, isPurchased);
        if (!result) return BadRequest("Ticket not found or invalid event");

        _rabbitMqService.PublishMessage("TicketUpdatedQueue", $"Ticket {ticketId} updated - Reserved: {isReserved}, Purchased: {isPurchased}");

        return Ok("Ticket availability updated successfully");
    }
}