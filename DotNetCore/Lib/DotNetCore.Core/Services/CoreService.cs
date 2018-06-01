using System;
using System.Reflection;
using System.Threading.Tasks;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Services
{
    public class CoreService : ICoreService
    {
        #region Constants

        private const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        #endregion

        public readonly IDataContext DataContext;
        public readonly IWorkContext WorkContext;

        public CoreService(IWorkContext workContext,IDataContext dataContext)
        {
            DataContext = dataContext;
            WorkContext = workContext;
        }

        public ResultMsg SaveChanges()
        {
            try
            {
                var lSavedResult = DataContext.SaveChanges();
                if (lSavedResult >= 1)
                    return CreateResultMsg("Success");
                if (lSavedResult == 0)
                    return CreateResultMsg("DataNotChange");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return CreateErrorMsg(e.Message);
            }
            catch (DbUpdateException e)
            {
                return CreateErrorMsg(e.Message);
            }
            catch (Exception e)
            {
                return CreateErrorMsg(e.Message);
            }
            return CreateErrorMsg("Fail");
        }

        public async Task<ResultMsg> SaveChangesAsync()
        {
            try
            {
                var lSavedResult = await DataContext.SaveChangesAsync();
                if (lSavedResult >= 1)
                    return CreateResultMsg("Success");
                if (lSavedResult == 0)
                    return CreateResultMsg("DataNotChange");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return CreateErrorMsg(e.Message);
            }
            catch (DbUpdateException e)
            {
                return CreateErrorMsg(e.Message);
            }
            catch (Exception e)
            {
                return CreateErrorMsg(e.Message);
            }
            return CreateErrorMsg("Fail");
        }

        public TModel Create<TModel>()
        {
            var lType = typeof(TModel);
            var lObject = Activator.CreateInstance(lType);

            var lPropInfo = lType.GetProperty("Id", BINDING_FLAGS);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(string))
                lPropInfo.SetValue(lObject, StringUtils.NewGuid());

            var lNowTime = DateTime.Now;
            lPropInfo = lType.GetProperty("Ctime", BINDING_FLAGS);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(DateTime))
                lPropInfo.SetValue(lObject, lNowTime);

            lPropInfo = lType.GetProperty("Mtime", BINDING_FLAGS);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(DateTime))
                lPropInfo.SetValue(lObject, lNowTime);

            lPropInfo = lType.GetProperty("Createdby", BINDING_FLAGS);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(string))
                lPropInfo.SetValue(lObject, UserId);

            lPropInfo = lType.GetProperty("Modifiedby", BINDING_FLAGS);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(string))
                lPropInfo.SetValue(lObject, UserId);

            return (TModel)lObject;
        }

        protected static ResultMsg CreateResultMsg(string message = null)
        {
            var lMsg = new ResultMsg
            {
                Status = 0,
                Message = message
            };
            return lMsg;
        }

        protected static ResultMsg CreateErrorMsg(string message = null)
        {
            var lMsg = new ResultMsg
            {
                Status = 1,
                Message = message
            };
            return lMsg;
        }

        protected string UserId => WorkContext.UserId;
    }
}
