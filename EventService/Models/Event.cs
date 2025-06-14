namespace EventService.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public List<TicketType> TicketTypes { get; set; }
    }

    public class TicketType
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }
    }

}
