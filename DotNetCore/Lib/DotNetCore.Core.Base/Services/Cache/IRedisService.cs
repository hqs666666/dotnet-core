 


/*****************************************************************************
 * 
 * Created On: 2018-05-25
 * Purpose:    redis manage
 * 
 ****************************************************************************/


using System;

namespace DotNetCore.Core.Base.Services.Cache
{
    public interface IRedisService : IDisposable
    {
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, int cacheTime);
        T Get<T>(string key);
        bool Remove(string key);
    }
}
