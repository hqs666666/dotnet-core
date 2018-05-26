using System;

namespace DotNetCore.FrameWork.Utils
{
    public class StringUtils
    {
        public static string NewGuid()
        {
            var lGuid = Guid.NewGuid().ToString("N");
            return lGuid;
        }
    }
}
