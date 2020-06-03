using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;
using LoggingManagement;

namespace MessagesManagement
{
    public class ItemPriceMessage
    {
        private readonly ILogService _logService;
        const string ServiceBusConnectionString = "Endpoint=sb://capstonesbevents.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9b+LZ9j16XL3Q9fD0RvUfRmVQWZ7jxiCfDd1y91gkaw=";
        const string TopicName = "pricechange";
        static ITopicClient topicClient;
        public static async Task SendMessagesAsync(string msg)
        {
            
            try
            {
                topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
                //string messageBody = JsonConvert.SerializeObject(orderEntity);
                Message message = new Message(Encoding.UTF8.GetBytes(msg));
                await topicClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                //_logService.LogException(exception);
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
