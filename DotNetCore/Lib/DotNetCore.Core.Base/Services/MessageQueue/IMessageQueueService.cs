using System;
using RabbitMQ.Client;

namespace DotNetCore.Core.Base.Services.MessageQueue
{
    public interface IMessageQueueService
    {
        void Publish<T>(string exchange, string queue, string routingKey, T body, string type = ExchangeType.Direct,
            bool isProperties = false) where T : class;

        void Subscribe<T>(string queue, Action<T> handler, bool isProperties = false) where T : class;

        void Pull<T>(string exchange, string queue, string routingKey, Action<T> handler,
            string type = ExchangeType.Direct) where T : class;
    }
}
