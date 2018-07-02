using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Base.Services.Log
{
    public interface ILogService : ICoreService
    {

        /* Log a message object */

        void Debug(object source, string message);
        void Debug(object source, string message, params object[] ps);
        void Debug(Type source, string message);
        void Info(object source, object message);
        void Info(Type source, object message);
        void Warn(object source, object message);
        void Warn(Type source, object message);
        void Error(object source, object message);
        void Error(Type source, object message);
        void Fatal(object source, object message);
        void Fatal(Type source, object message);

        /* Log a message object and exception */

        void Debug(object source, object message, Exception exception);

        void Debug(Type source, object message, Exception exception);

        void Info(object source, object message, Exception exception);

        void Info(Type source, object message, Exception exception);

        void Warn(object source, object message, Exception exception);

        void Warn(Type source, object message, Exception exception);
        
        void Error(object source, object message, Exception exception);

        void Error(Type source, object message, Exception exception);

        void Fatal(object source, object message, Exception exception);

        void Fatal(Type source, object message, Exception exception);

        List<Domain.Log.Log> GetList();
    }
}
