using System.Text;
using CalifornianHealthLib.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CalifornianHealthLib.RabbitMq
{
    public interface IRabbitMqPublisher
    {
        void PublishBookingMessage(Query message, string name, string hostName);
    }
    
    public class RabbitMqBookingRequestPublisher : IRabbitMqPublisher
    {
        public void PublishBookingMessage(Query message, string name, string hostName)
        {
            var factory = RabbitMqConfiguration.GetConnectionFactory(hostName);
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var queueName = name;

            channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (queueName != "ch_queue_test_request" || queueName != "ch_queue_test_response")
            {
                Console.WriteLine($" [x] Publisher -> Sent booking {json} to {queueName}");
            }

            Console.ForegroundColor = ConsoleColor.Black;
            channel.BasicPublish(exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}