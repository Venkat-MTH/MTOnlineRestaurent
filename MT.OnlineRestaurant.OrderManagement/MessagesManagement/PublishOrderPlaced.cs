using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;
using LoggingManagement;

namespace MessagesManagement
{
    public class PublishOrderPlaced
    {
        private readonly ILogService _logService;
        const string ServiceBusConnectionString = "Endpoint=sb://capstonesbevents.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KYDlMTGUX4tzIB4WEEAO/BgKRKPoZzl0LhXyMSghrkc=";
        const string TopicName = "orderedplaced";
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
