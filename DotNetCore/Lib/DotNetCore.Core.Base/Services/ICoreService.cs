
namespace DotNetCore.Core.Base.Services
{
    public interface ICoreService
    {
        ResultMsg SaveChanges();
        TModel Create<TModel>();
    }
}
