

/*****************************************************************************
 * 
 * Created On: 2018-05-31
 * Purpose:    图片相关
 * 
 ****************************************************************************/

using System.Threading.Tasks;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Domain.File;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Services.User
{
    public class PicResourceService : CoreService, IPicResourceService
    {
        #region Ctor

        private readonly DbSet<PicRelate> mPicRelateDbSet;
        private readonly DbSet<PicResource> mPicResourceDbSet;

        public PicResourceService(IWorkContext workContext, IDataContext dataContext)
            : base(workContext, dataContext)
        {
            mPicRelateDbSet = DataContext.Set<PicRelate>();
            mPicResourceDbSet = DataContext.Set<PicResource>();
        }

        #endregion

        public async Task<ResultMsg> Save()
        {
            var lRelate = Create<PicRelate>();
            var lResult = await SaveChangesAsync();
            if (lResult.Result)
                lResult.Data = lRelate.Id;
            return lResult;
        }
    }
}
