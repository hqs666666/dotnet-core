


/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    API 调用结果的消息封装类。
 * 
 ****************************************************************************/

using Newtonsoft.Json;

namespace DotNetCore.Core.Base
{
    /// <summary>
    /// API 调用结果的消息封装类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultMsg<T>
    {
        /// <summary>
        /// 详见 ApiErrorCode 定义
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 错误消息，默认取自 ApiErrorCode 的描述
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 返回的业务数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 可供客户端访问以获取更多信息的地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 结果状态的简单标识
        /// </summary>
        [JsonIgnore]
        public bool Result => ErrorCode == (int)ApiErrorCode.Success;
    }
}
