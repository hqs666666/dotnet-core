 

/*****************************************************************************
 * 
 * Created On: 2018-07-04
 * Purpose:    事件发布
 * 
 ****************************************************************************/


namespace DotNetCore.Core.Base.Services.MessageQueue
{
    public interface IEventPublish
    {
        void SendEmail<T>(T message) where T : class;
    }
}
