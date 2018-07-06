

/*****************************************************************************
 * 
 * Created On: 2018-07-04
 * Purpose:    Rabbit
 * 
 ****************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.Core.Base.Services.MessageQueue;
using DotNetCore.FrameWork.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotNetCore.Core.Services.MessageQueue
{
    public class RabbitMqService : IMessageQueueService
    {
        private readonly IConfigService mConfigService;
        private readonly ILogService mLogService;

        public RabbitMqService(IConfigService configService, ILogService logService)
        {
            mConfigService = configService;
            mLogService = logService;
            Open();
        }

        //RabbitMQ建议客户端线程之间不要共用Model，至少要保证共用Model的线程发送消息必须是串行的，但是建议尽量共用Connection。
        private static readonly ConcurrentDictionary<string, IModel> _modelDic =
            new ConcurrentDictionary<string, IModel>();

        private static IConnection _conn;

        private static readonly object _lockMq = new object();

        private void Open()
        {
            if (_conn != null) return;
            lock (_lockMq)
            {
                var lFactory = new ConnectionFactory
                {
                    HostName = mConfigService.RabbitMqHostName,
                    UserName = mConfigService.RabbitMqUserName,
                    Password = mConfigService.RabbitMqPwd,
                    RequestedHeartbeat = 60, //设置心跳时间
                    AutomaticRecoveryEnabled = true, //设置自动重连
                    NetworkRecoveryInterval = new TimeSpan(1000) //重连时间
                };
                _conn = _conn ?? lFactory.CreateConnection();
            }
        }

        #region 声明交换器

        /// <summary>
        /// 交换器声明
        /// </summary>
        /// <param name="iModel"></param>
        /// <param name="exchange">交换器</param>
        /// <param name="type">交换器类型：
        /// 1、Direct Exchange – 处理路由键。需要将一个队列绑定到交换机上，要求该消息与一个特定的路由键完全
        /// 匹配。这是一个完整的匹配。如果一个队列绑定到该交换机上要求路由键 “dog”，则只有被标记为“dog”的
        /// 消息才被转发，不会转发dog.puppy，也不会转发dog.guard，只会转发dog
        /// 2、Fanout Exchange – 不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都
        /// 会被转发到与该交换机绑定的所有队列上。很像子网广播，每台子网内的主机都获得了一份复制的消息。Fanout
        /// 交换机转发消息是最快的。
        /// 3、Topic Exchange – 将路由键和某模式进行匹配。此时队列需要绑定要一个模式上。符号“#”匹配一个或多
        /// 个词，符号“*”匹配不多不少一个词。因此“audit.#”能够匹配到“audit.irs.corporate”，但是“audit.*”
        /// 只会匹配到“audit.irs”。</param>
        /// <param name="durable">持久化</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        private static void ExchangeDeclare(IModel iModel, string exchange, string type,
            bool durable = true, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            exchange = string.IsNullOrWhiteSpace(exchange) ? "" : exchange.Trim();
            iModel.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }

        #endregion

        #region 声明队列

        /// <summary>
        /// 队列声明
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queue">队列</param>
        /// <param name="durable">持久化</param>
        /// <param name="exclusive">排他队列，如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，
        /// 并在连接断开时自动删除。这里需要注意三点：其一，排他队列是基于连接可见的，同一连接的不同信道是可
        /// 以同时访问同一个连接创建的排他队列的。其二，“首次”，如果一个连接已经声明了一个排他队列，其他连
        /// 接是不允许建立同名的排他队列的，这个与普通队列不同。其三，即使该队列是持久化的，一旦连接关闭或者
        /// 客户端退出，该排他队列都会被自动删除的。这种队列适用于只限于一个客户端发送读取消息的应用场景。</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        private static void QueueDeclare(IModel channel, string queue, bool durable = true, bool exclusive = false,
            bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            queue = string.IsNullOrWhiteSpace(queue) ? "UndefinedQueueName" : queue.Trim();
            channel.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

        #endregion

        #region 获取Model

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey"></param>
        /// <param name="type">交换器类型</param>
        /// <param name="isProperties">是否持久化</param>
        /// <returns></returns>
        private static IModel GetModel(string exchange, string queue, string routingKey,string type, bool isProperties = false)
        {
            return _modelDic.GetOrAdd(queue, key =>
            {
                var lModel = _conn.CreateModel();
                ExchangeDeclare(lModel, exchange, type, isProperties);
                QueueDeclare(lModel, queue, isProperties);
                lModel.QueueBind(queue, exchange, routingKey);
                _modelDic[queue] = lModel;
                return lModel;
            });
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties"></param>
        /// <returns></returns>
        private static IModel GetModel(string queue, bool isProperties)
        {
            return _modelDic.GetOrAdd(queue, value =>
            {
                var lModel = _conn.CreateModel();
                QueueDeclare(lModel, queue, isProperties);

                //每次消费的消息数
                lModel.BasicQos(0, 1, false);

                _modelDic[queue] = lModel;

                return lModel;
            });
        }
        #endregion

        #region 发布消息

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">队列信息</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名</param>
        /// <param name="isProperties">是否持久化</param>
        /// <param name="type">交换器类型</param>
        /// <returns></returns>
        public void Publish<T>(string exchange, string queue, string routingKey, T body, string type = ExchangeType.Direct, bool isProperties = false) where T : class
        {
            var lChannel = GetModel(exchange, queue, routingKey, type, isProperties);

            try
            {
                var lBody = JsonHelper.ObjectToBytes(body);
                lChannel.BasicPublish(exchange, routingKey, null, lBody);
            }
            catch (Exception lEx)
            {
                mLogService.Error(this, $"发布消息失败：exchange:{exchange},queue:{queue},routiinKey:{routingKey}", lEx);
                throw new Exception(lEx.Message);
            }
        }

        #endregion

        #region 订阅消息

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties">是否持久化</param>
        /// <param name="handler">消费处理</param>
        public void Subscribe<T>(string queue, Action<T> handler, bool isProperties = false) where T : class
        {
            //队列声明
            var lChannel = GetModel(queue, isProperties);

            var lConsumer = new EventingBasicConsumer(lChannel);
            lConsumer.Received += (model, ea) =>
            {
                var lBody = ea.Body;
                var lMsg = JsonHelper.BytesToObject<T>(lBody);
                try
                {
                    handler(lMsg);
                }
                catch (Exception lEx)
                {
                    mLogService.Error(this,$"接收消息成功，处理事件失败，queue：{queue}",lEx);
                }
                finally
                {
                    lChannel.BasicAck(ea.DeliveryTag, false);
                }
            };
            lChannel.BasicConsume(queue, false, lConsumer);
        }

        #endregion

        #region 拉取消息（主动）

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="handler">消费处理</param>
        /// <param name="type">交换器类型</param>
        public void Pull<T>(string exchange, string queue, string routingKey, Action<T> handler,string type = ExchangeType.Direct) where T : class
        {
            var lChannel = GetModel(exchange, queue, routingKey,type);

            var lResult = lChannel.BasicGet(queue, false);
            if (lResult == null)
                return;

            var lMsg = JsonHelper.BytesToObject<T>(lResult.Body);
            try
            {
                handler(lMsg);
            }
            catch (Exception lEx)
            {
                mLogService.Error(this, $"拉取消息失败，exchange：{exchange}，queue：{queue}", lEx);
            }
            finally
            {
                lChannel.BasicAck(lResult.DeliveryTag, false);
            }
        }

        #endregion

    }
}
