using LoggingManagement;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Buffers;
using Microsoft.Azure.ServiceBus;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MT.Online.Restaurant.MessagesManagement.Services
{
    public class ConsumeOutOfStock : IConsumeOutOfStock
    {
        const string ServiceBusConnectionString = "Endpoint=sb://capstonesbevents.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9b+LZ9j16XL3Q9fD0RvUfRmVQWZ7jxiCfDd1y91gkaw=";
        const string TopicName = "itemoutofstock";
        const string SubscriptionName = "s1";
        static ISubscriptionClient subscriptionClient;
        public ICartActions _cartBusiness { get; set; }
        private readonly ILogService _logService;
        public ConsumeOutOfStock(ICartActions cartBusiness, ILogService logService)
        {
            _cartBusiness = cartBusiness;
            _logService = logService;
        }
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
            //string message = "Sent back response";
            //SendMessage.SendMessagesAsync(message);
        }
        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            string msg = Encoding.UTF8.GetString(message.Body);
            //OrderMenus msg = JsonConvert.DeserializeObject<OrderMenus>(Encoding.UTF8.GetString(message.Body));
            await _cartBusiness.UpdateItemoutofstock(msg);
            //string msg = Encoding.UTF8.GetString(message.Body);
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
            _logService.LogMessage("Outofstock Event consumed: " + msg);
        }

         Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            _logService.LogMessage("Outofstock Event exception: " + exceptionReceivedEventArgs.ExceptionReceivedContext);
            return Task.CompletedTask;
        }
       
    }
   
}
