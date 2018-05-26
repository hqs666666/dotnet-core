


/*****************************************************************************
 * 
 * Created On: 2018-05-25
 * Purpose:    redis manage
 * 
 ****************************************************************************/

using DotNetCore.Core.Base.Services.Cache;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DotNetCore.Core.Services.Cache
{
    public class RedisService : IRedisService
    {
        private RedisService()
        {

        }

        private bool mDisposed;
        private static ConnectionMultiplexer _instance;
        private static readonly object _locker = new object();
        /// <summary>
        /// 单例模式获取redis连接实例
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                lock (_locker)
                {

                    if (_instance == null)
                    {
                        if (_instance == null)
                            _instance = ConnectionMultiplexer.Connect("127.0.0.1"); //这里应该配置文件，不过这里演示就没写
                    }
                }
                return _instance;
            }
        }

        public void Set<T>(string key, T value)
        {
            var lRedis = Instance.GetDatabase();
            var lHasKey = lRedis.KeyExists(key);
            if (lHasKey)
                return;
            var lValue = JsonConvert.SerializeObject(value);
            lRedis.StringSet(key, lValue);
        }

        public T Get<T>(string key)
        {
            var lRedis = Instance.GetDatabase();
            var lHasKey = lRedis.KeyExists(key);
            if (!lHasKey)
                return default(T);
            var lVaule = lRedis.StringGet(key);
            return JsonConvert.DeserializeObject<T>(lVaule);
        }

        public bool Remove(string key)
        {
            var lRedis = Instance.GetDatabase();
            var lHasKey = lRedis.KeyExists(key);
            if (!lHasKey)
                return true;

            return lRedis.SetRemove(key, lRedis.StringGet(key));
        }

        public void Dispose()
        {
            if (mDisposed) return;

            //if (disposing)
            //{
            //}
            mDisposed = true;
        }
    }
}
