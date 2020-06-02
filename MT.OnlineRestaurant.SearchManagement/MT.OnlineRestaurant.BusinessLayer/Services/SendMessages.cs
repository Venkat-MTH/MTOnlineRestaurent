using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.BusinessEntities;
using Newtonsoft.Json;

namespace MT.Online.Restaurant.BusinessLayer.Services
{
    public class SendMessages
    {
        const string ServiceBusConnectionString = "Endpoint=sb://capstonesb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=H6qStKOKlEXrvTUO9Mk8CD7Sd2WBx2i6rAWAfEb8tSI=";
        const string TopicName = "itemoutofstock";
        static ITopicClient topicClient;
        public static async Task SendMessagesAsync(OrderEntity orderEntity)
        {
            try
            {
                topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
                string messageBody = JsonConvert.SerializeObject(orderEntity);
                Message message = new Message(Encoding.UTF8.GetBytes(messageBody));
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
