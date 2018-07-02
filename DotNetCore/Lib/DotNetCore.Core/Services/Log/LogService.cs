using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.FrameWork.Helpers;
using DotNetCore.FrameWork.Utils;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Services.Log
{
    public class LogService : CoreService, ILogService
    {
        private readonly ConcurrentDictionary<Type, ILog> mLoggers = new ConcurrentDictionary<Type, ILog>();
        private ILoggerRepository Repository { get; set; }
        private readonly IHttpContextAccessor mHttpContextAccessor;
        private readonly DbSet<Domain.Log.Log> mLogDbSet;

        public LogService(IWorkContext workContext, IDataContext dataContext,
            IHttpContextAccessor httpContextAccessor)
        : base(workContext, dataContext)
        {
            var lRepositories = LogManager.GetAllRepositories();
            if (lRepositories.Any(p => p.Name == "NETCoreRepository"))
                Repository = LogManager.GetRepository("NETCoreRepository");
            else
                Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));

            mHttpContextAccessor = httpContextAccessor;
            mLogDbSet = DataContext.Set<Domain.Log.Log>();
        }

        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        private ILog GetLogger(Type source)
        {
            if (mLoggers.ContainsKey(source))
            {
                return mLoggers[source];
            }
            else
            {
                var lLogger = LogManager.GetLogger(Repository.Name, source);
                mLoggers.TryAdd(source, lLogger);
                return lLogger;
            }
        }

        /* Log a message object */

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Debug(object source, string message)
        {
            Debug(source.GetType(), message);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="ps">ps</param>
        public void Debug(object source, string message, params object[] ps)
        {
            Debug(source.GetType(), string.Format(message, ps));
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Debug(Type source, string message)
        {
            var lLogger = GetLogger(source);
            if (lLogger.IsDebugEnabled)
            {
                lLogger.Debug(message);
                SaveInDb("debug", message);
            }
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Info(Type source, object message)
        {
            var lLogger = GetLogger(source);
            if (lLogger.IsInfoEnabled)
            {
                lLogger.Info(message);
                SaveInDb("info", message);
            }
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Warn(Type source, object message)
        {
            var lLogger = GetLogger(source);
            if (lLogger.IsWarnEnabled)
            {
                lLogger.Warn(message);
                SaveInDb("warn", message);
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Error(Type source, object message)
        {
            var lLogger = GetLogger(source);
            if (lLogger.IsErrorEnabled)
            {
                lLogger.Error(message);
                SaveInDb("error", message);
            }
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public void Fatal(Type source, object message)
        {
            var lLogger = GetLogger(source);
            if (lLogger.IsFatalEnabled)
            {
                lLogger.Fatal(message);
                SaveInDb("fatal", message);
            }
        }

        /* Log a message object and exception */

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Debug(Type source, object message, Exception exception)
        {
            GetLogger(source).Debug(message, exception);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Info(Type source, object message, Exception exception)
        {
            GetLogger(source).Info(message, exception);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Warn(Type source, object message, Exception exception)
        {
            GetLogger(source).Warn(message, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Error(Type source, object message, Exception exception)
        {
            GetLogger(source).Error(message, exception);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public void Fatal(Type source, object message, Exception exception)
        {
            GetLogger(source).Fatal(message, exception);
        }

        public List<Domain.Log.Log> GetList()
        {
            var lToday = DateTime.Today;
            var lTomorrow = lToday.AddDays(1);
            return mLogDbSet.Where(p => p.Ctime >= lToday && p.Ctime <= lTomorrow).ToList();
        }
        private void SaveInDb(string type,object desc)
        {
            var lLog = new Domain.Log.Log
            {
                CreatedBy = UserId,
                Ctime = DateTime.Now,
                Ip = Utils.GetIP(),
                LogType = type,
                Url = mHttpContextAccessor.HttpContext.Request.Host + mHttpContextAccessor.HttpContext.Request.Path,
                Description = JsonHelper.ObjectToJson(desc),
            };
            mLogDbSet.Add(lLog);
            SaveChanges();
        }
    }
}
