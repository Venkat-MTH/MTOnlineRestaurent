using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.BusinessEntities;
using Newtonsoft.Json;
using MT.OnlineRestaurant.DataLayer.Context;
using LoggingManagement;

namespace MT.Online.Restaurant.MessagesManagement.Services
{
    public class ConsumePriceChange : IConsumePriceChange
    {
        public ICartActions _cartBusiness { get; set; }
        private readonly ILogService _logService;
        public ConsumePriceChange(ICartActions cartBusiness, ILogService logService)
        {
            _logService = logService;
            _cartBusiness = cartBusiness;
        }
        const string ServiceBusConnectionString = "Endpoint=sb://capstonesbevents.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9b+LZ9j16XL3Q9fD0RvUfRmVQWZ7jxiCfDd1y91gkaw=";
        const string TopicName = "pricechange";
        const string SubscriptionName = "ps1";
        static ISubscriptionClient subscriptionClient;
        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            string msg = Encoding.UTF8.GetString(message.Body);
            msg = msg.Replace("Id", "TblMenuID");
            CartItemsEntity cart = (CartItemsEntity)JsonConvert.DeserializeObject<CartItemsEntity>(msg);

            await _cartBusiness.UpdateCartMenuItemPrice(cart);
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            _logService.LogMessage("Pricechange Event consumed: " + cart);
        }

        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            _logService.LogMessage("Pricechange Event exception: " + exceptionReceivedEventArgs.ExceptionReceivedContext);
            return Task.CompletedTask;
        }


    }
}
