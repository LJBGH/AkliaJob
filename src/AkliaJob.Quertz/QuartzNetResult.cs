using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.Quertz
{
    public class QuartzNetResult
    {
        public QuartzNetResult(string msg, bool success=true)
        {
            Msg = msg;
            Success = success;
        }

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
