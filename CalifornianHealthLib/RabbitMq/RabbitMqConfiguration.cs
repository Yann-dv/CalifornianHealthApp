using RabbitMQ.Client;

namespace CalifornianHealthLib.RabbitMq
{

    public static class RabbitMqConfiguration
    {
        public static ConnectionFactory GetConnectionFactory(string? hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName) || hostName == string.Empty)
            {
                hostName = "localhost";
            }
            return new ConnectionFactory()
            {
                HostName = hostName,
                UserName = "ch_queue_user",
                Password = "ch_pwd_123",
                Port = 5672,
                VirtualHost = "/"
            };
        }
    }
}