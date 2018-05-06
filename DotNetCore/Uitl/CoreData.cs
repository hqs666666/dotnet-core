using DotNetCore.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;

namespace DotNetCore.Uitl
{
    public class CoreData
    {
        private readonly EFCoreContext Context;

        public CoreData(EFCoreContext context)
        {
            Context = context;
        }

        public ResultMsg SaveChanges()
        {
            try
            {
                var savedResult = Context.SaveChanges();
                if (savedResult >= 1)
                    return CreateResultMsg("Success");
                if (savedResult == 0)
                    return CreateResultMsg("DataNotChange");
            }
            catch (DbUpdateConcurrencyException e)
            {
                Log.Error(e);
                return CreateErrorMsg(e.Message);
            }
            catch (DbUpdateException e)
            {
                Log.Error(e);
                return CreateErrorMsg(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return CreateErrorMsg(e.Message);
            }
            return CreateErrorMsg("Fail");
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


        private ILog Log = LogManager.GetLogger(Startup.Repository.Name, typeof(CoreData));
    }
}
