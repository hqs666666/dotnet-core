

/*****************************************************************************
 * 
 * Created On: 2018-06-15
 * Purpose:    事件生产者
 * 
 ****************************************************************************/

using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Helpers;
using DotNetCore.FrameWork.Utils;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DotNetCore.Core.Events
{
    public class EventProducer
    {
        public void Publisher<T>(T body)
        {
            using (var lConnection = Factory.CreateConnection())
            {
                using (var lChannel = lConnection.CreateModel())
                {
                    //定义一个Direct类型交换机
                    lChannel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, false, false, null);

                    //定义一个队列
                    lChannel.QueueDeclare(QueueName, true, false, false, null);

                    //将队列绑定到交换机
                    lChannel.QueueBind(QueueName, ExchangeName, RouteKey, null);

                    var lSendBytes = JsonHelper.ObjectToBytes(body);

                    //发布消息
                    lChannel.BasicPublish(ExchangeName, RouteKey, null, lSendBytes);
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
        private readonly string ExchangeName = "exchange";
        private readonly string QueueName = "queue";
        private readonly string RouteKey = "routeKey";
    }
}
