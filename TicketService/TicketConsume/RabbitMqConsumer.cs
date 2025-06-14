using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketService.Models;
using TicketService.TicketConsume;

namespace TicketService.TicketConsume
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly TicketContext _context;
        private readonly string _queueName;

        public RabbitMqConsumer(TicketContext context, IConfiguration configuration)
        {
            _context = context;
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]

            };
            _queueName = configuration["RabbitMQ:QueueName"];

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async(model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received Event: {message}");
                await UpdateTicketAvailabilityBasedOnEvent(message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Keep the service running
            }
        }
        private async Task UpdateTicketAvailabilityBasedOnEvent(string message)
        {
            Console.WriteLine($"Updating ticket availability based on event: {message}");
            var messageParts = message.Split(',');
            var eventIdPart = messageParts.FirstOrDefault(p => p.Contains("EventID"));
            var capacityPart = messageParts.FirstOrDefault(p => p.Contains("Capacity"));

            if (eventIdPart == null || capacityPart == null)
            {
                return;
            }

            int eventId = int.Parse(eventIdPart.Split(':')[1]);
            int newCapacity = int.Parse(capacityPart.Split(':')[1]);
            var tickets = await _context.Tickets.Where(t => t.EventId == eventId).ToListAsync();

            if (tickets == null || !tickets.Any())
            {
                return;
            }
            foreach (var ticket in tickets)
            {
                if (newCapacity == 0)
                {
                    ticket.IsPurchased = true;
                }
            }

            _context.Tickets.UpdateRange(tickets);
            await _context.SaveChangesAsync();
        }
        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }
    }
}