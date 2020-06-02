using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MT.Online.Restaurant.MessagesManagement.Services
{
    public interface IConsumePriceChange
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        //void SendMessagesAsync<T>(T senderObj) where T : class;
        //void SendMessagesAsync(string msg);
    }
}
