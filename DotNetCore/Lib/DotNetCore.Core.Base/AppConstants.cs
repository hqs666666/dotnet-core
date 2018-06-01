/*****************************************************************************
 * 
 * Created On: 2018-05-10
 * Purpose:    常用常量集合
 * 
 ****************************************************************************/

namespace DotNetCore.Core.Base
{
    public sealed class AppConstants
    {
        #region 未授权时默认信息

        public const string USER_ID = "-1";
        public const string NAME = "sb";
        public const string NICK_NAME = "sb";
        public const string ROLE = "Guest";

        #endregion

        #region Role

        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_REGISTER_USER = "RegisterUser";

        #endregion

        #region 静态文件路径

        public const string FILE_PICTURE_URL = @"\Static\Pictures\";

        #endregion

    }
}
