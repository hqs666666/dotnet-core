 

/*****************************************************************************
 * 
 * Created On: 2018-07-04
 * Purpose:    事件发布
 * 
 ****************************************************************************/

using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services.MessageQueue;

namespace DotNetCore.Core.Services.MessageQueue
{
    public class EventPublish : IEventPublish
    {
        private readonly IMessageQueueService mMessageQueueService;

        public EventPublish(IMessageQueueService messageQueueService)
        {
            mMessageQueueService = messageQueueService;
        }

        public void SendEmail<T>(T message) where T : class
        {
            var lExchange = AppConstants.SEND_EMAIL_EXCHANGE_NAME;
            var lQueue = AppConstants.SEND_EMAIL_QUEUE_NAME;
            var lRouteKey = AppConstants.SEND_EMAIL_ROUTING_KEY;
            mMessageQueueService.Publish(lExchange, lQueue, lRouteKey, message);
        }
    }
}
