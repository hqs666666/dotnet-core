 

/*****************************************************************************
 * 
 * Created On: 2018-06-15
 * Purpose:    事件消费者
 * 
 ****************************************************************************/

using System;
using System.Text;
using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Helpers;
using DotNetCore.FrameWork.Utils;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotNetCore.Core.Events
{
    public class EventsConsumer
    {
        public T Subscribe<T>()
        {
            using (var lConnection = Factory.CreateConnection())
            {
                using (var lChannel = lConnection.CreateModel())
                {
                    var lResult = lChannel.BasicGet(QueueName, false);
                    if (lResult == null)
                        return default(T);

                    lChannel.BasicAck(lResult.DeliveryTag, false);
                    var lMessage = JsonHelper.BytesToObject<T>(lResult.Body);
                    return lMessage;
                }
            }
        }

        public void HandleEvent()
        {
            using (var lConnection = Factory.CreateConnection())
            {
                using (var lChannel = lConnection.CreateModel())
                {
                    //事件基本消费者
                    var lConsumer = new EventingBasicConsumer(lChannel);

                    //接收到消息事件
                    lConsumer.Received += (ch, ea) =>
                    {
                        var lMessage = Encoding.UTF8.GetString(ea.Body);

                        //发布消息后，该方法会主动接收到消息，此处可以处理一些事

                        //确认该消息已被消费
                        lChannel.BasicAck(ea.DeliveryTag, false);

                    };
                    //启动消费者 设置为手动应答消息
                    lChannel.BasicConsume("queueName", false, lConsumer);
                    Console.WriteLine("消费者已启动");
                }
            }
        }

        private IConfigService ConfigService => DI.ServiceProvider.GetRequiredService<IConfigService>();
        private ConnectionFactory Factory => new ConnectionFactory()
        {
            UserName = ConfigService.RabbitMqUserName,
            Password = ConfigService.RabbitMqPwd,
            HostName = ConfigService.RabbitMqHostName
        };
        private readonly string QueueName = "queue";
    }
}
