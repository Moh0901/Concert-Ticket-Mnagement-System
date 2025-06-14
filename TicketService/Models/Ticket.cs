namespace TicketService.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservationTime { get; set; }
        public bool IsPurchased { get; set; }
    }

}
