using CalifornianHealthLib.Models;
using CalifornianHealthLib.RabbitMq;
using Newtonsoft.Json;

namespace CalifornianHealthFrontendUpdated.Services
{
    public class ResponseConsumer : RabbitMqBookingRequestConsumer
    {
        
        protected override Task<string> GetMessage(string message, string confirmationId)
        {
            var query = JsonConvert.DeserializeObject<Query>(message);
            if (query.MessageId.ToString() != confirmationId)
            {
                return Task.FromResult<string>(null);
            }
            
            Console.WriteLine(" [+] Response received and well updated from booking request, message Id: " + query.MessageId);
            return Task.FromResult(query.MessageId.ToString());
        }
    }
}