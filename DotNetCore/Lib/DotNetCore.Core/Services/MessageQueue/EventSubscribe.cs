


/*****************************************************************************
 * 
 * Created On: 2018-07-04
 * Purpose:    事件订阅
 * 
 ****************************************************************************/

using DotNetCore.Core.Base;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services.Message;
using DotNetCore.Core.Base.Services.MessageQueue;

namespace DotNetCore.Core.Services.MessageQueue
{
    public class EventSubscribe : IEventSubscribe
    {
        private readonly IMessageQueueService mMessageQueueService;
        private readonly IEmailService mEmailService;

        public EventSubscribe(IMessageQueueService messageQueueService, IEmailService emailService)
        {
            mMessageQueueService = messageQueueService;
            mEmailService = emailService;
        }

        public void SendEmail()
        {
            var lQueueName = AppConstants.SEND_EMAIL_QUEUE_NAME;
            mMessageQueueService.Subscribe<UserDto>(lQueueName, user =>
            {
                mEmailService.VaildEmail(user);
            });
        }
    }
}
