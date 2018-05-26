

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    API 错误码定义
 * 
 ****************************************************************************/

using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Core.Base
{
    public enum ApiErrorCode
    {
        [Display(Name = "系统繁忙，稍后再试")]
        SystemBusy = -1,

        [Display(Name = "请求成功")]
        Success = 0,

        [Display(Name = "处理请求异常")]
        Exception = 1,
    }
}
