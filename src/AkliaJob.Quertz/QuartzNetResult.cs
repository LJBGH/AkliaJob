using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.Quertz
{
    public class QuartzNetResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
    }
}
