 


/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    常用枚举集合
 * 
 ****************************************************************************/


using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Core.Base
{
    public enum UserType
    {
        [Display(Name = "普通用户")]
        Common = 0,

        [Display(Name = "管理用户")]
        Manage = 1,
    }

    public enum Gender
    {
        [Display(Name = "未知")]
        Unknown = 0,

        [Display(Name = "男")]
        Male = 1,

        [Display(Name = "女")]
        Female = 2
    }
}
