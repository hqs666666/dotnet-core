﻿


/*****************************************************************************
 * 
 * Created On: 2018-05-25
 * Purpose:    redis manage
 * 
 ****************************************************************************/

using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.Cache;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using DotNetCore.FrameWork.Helpers;
using DotNetCore.FrameWork.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Core.Services.Cache
{
    public class RedisService : IRedisService
    {
        private static ConnectionMultiplexer _instance;
        private static readonly object _locker = new object();
        private static IConfigService ConfigService => DI.ServiceProvider.GetRequiredService<IConfigService>();
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
                        _instance = ConnectionMultiplexer.Connect(ConfigService.RedisConnection);
                }
                return _instance;
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var lRedis = Instance.GetDatabase();
            var lValue = JsonHelper.ObjectToBytes(value);
            await lRedis.StringSetAsync(key, lValue);
        }

        public async Task SetAsync<T>(string key, T value, int cacheTime)
        {
            if (null == value)
                return;

            var lValue = JsonHelper.ObjectToBytes(value);
            var lExpiresIn = TimeSpan.FromMinutes(cacheTime);

            var lRedis = Instance.GetDatabase();
            await lRedis.StringSetAsync(key, lValue, lExpiresIn);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var lRedis = Instance.GetDatabase();
            var lHasKey = await lRedis.KeyExistsAsync(key);
            if (!lHasKey)
                return default(T);
            var lVaule = await lRedis.StringGetAsync(key);
            return JsonHelper.BytesToObject<T>(lVaule);
        }

        public T Get<T>(string key)
        {
            var lRedis = Instance.GetDatabase();
            var lHasKey = lRedis.KeyExists(key);
            if (!lHasKey)
                return default(T);
            var lVaule = lRedis.StringGet(key);
            return JsonHelper.BytesToObject<T>(lVaule);
        }

        public async Task RemoveAsync(string key)
        {
            var lRedis = Instance.GetDatabase();
            var lHasKey = await lRedis.KeyExistsAsync(key);
            if (!lHasKey)
                return;

            await lRedis.SetRemoveAsync(key, lRedis.StringGet(key));
        }

        public void Dispose()
        {
               
        }
    }
}
