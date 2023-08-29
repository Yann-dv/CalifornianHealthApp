using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CalifornianHealthLib.RabbitMq
{
    public interface IRabbitMqConsumer
    {
        void ReceiveBookingMessage(List<string> queueList, string hostName); //, string messageId);
    }
    public class RabbitMqBookingRequestConsumer : IRabbitMqConsumer
    {
        public void ReceiveBookingMessage(List<string> queueList, string hostName)
        {
            foreach (var queue in queueList)
            {
                var factory = RabbitMqConfiguration.GetConnectionFactory(hostName);
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                var queueName = queue;

                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($" [*] Listener -> Waiting for {queueName}s requests...");
                Console.ForegroundColor = ConsoleColor.Black;

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    try
                    {
                        if (queueList.Contains("ch_queue_test_response") || queueList.Contains("ch_queue_booking_response"))
                        {
                            Console.WriteLine(" [+] Response from " + queueName + " received: " + message);
                        }
                        else
                        {
                            await UpdateBookings(message, queueName);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Unable to update the db from received message: " + e);
                        Console.ForegroundColor = ConsoleColor.Black;
                        throw;
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
        }

        /// <summary>
        /// To be overriden by the consumer in BookingServer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        protected virtual async Task<bool> UpdateBookings(string message, string queueName)
        {
            return false;
        }
        
        protected virtual async Task<string> GetMessage(string message, string messageId)
        {
            return "no new message";
        }
    }
}