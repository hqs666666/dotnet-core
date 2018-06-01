

/*****************************************************************************
 * 
 * Created On: 2018-05-31
 * Purpose:    图片相关
 * 
 ****************************************************************************/

using System.Threading.Tasks;

namespace DotNetCore.Core.Base.Services.User
{
    public interface IPicResourceService : ICoreService
    {
        Task<ResultMsg> Save();
    }
}
