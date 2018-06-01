
using System.Threading.Tasks;

namespace DotNetCore.Core.Base.Services
{
    public interface ICoreService
    {
        ResultMsg SaveChanges();
        Task<ResultMsg> SaveChangesAsync();
        TModel Create<TModel>();
    }
}
