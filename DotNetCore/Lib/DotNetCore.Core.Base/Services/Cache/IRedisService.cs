 


/*****************************************************************************
 * 
 * Created On: 2018-05-25
 * Purpose:    redis manage
 * 
 ****************************************************************************/


using System;
using System.Threading.Tasks;

namespace DotNetCore.Core.Base.Services.Cache
{
    public interface IRedisService : IDisposable
    {
        Task SetAsync<T>(string key, T value);
        Task SetAsync<T>(string key, T value, int cacheTime);
        Task<T> GetAsync<T>(string key);
        T Get<T>(string key);
        Task RemoveAsync(string key);
    }
}
