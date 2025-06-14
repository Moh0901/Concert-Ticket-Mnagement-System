using RabbitMQ.Client;
using System.Text;

namespace EventService.Producer
{
    public class EventMessageProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        public EventMessageProducer(IConfiguration configuration)
        {
            _factory = new ConnectionFactory() { HostName = configuration["RabbitMQ:HostName"] };
            _queueName = configuration["RabbitMQ:QueueName"];
        }

        public void PublishMessage(string message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}