using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Base
{
    /// <summary>
    /// 返回结果的消息基类定义
    /// </summary>
    [Serializable]
    public abstract class ResultMsgBase
    {
        /// <summary>
        /// Status 为状态代码， 一般情况下 0 表示没有错误， 非 0 表示有错误
        /// </summary>
        public uint Status { get; set; }

        public string Message { get; set; }

        public bool Result => Status == 0;
    }

    /// <summary>
    /// 返回结果的消息类定义
    /// </summary>
    [Serializable]
    public class ResultMsg : ResultMsgBase
    {
        public object Data { get; set; }
    }
}
