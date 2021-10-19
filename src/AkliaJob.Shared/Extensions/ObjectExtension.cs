using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.Shared
{
    public static class ObjectExtension
    {

        /// <summary>
        /// 验证指定值的断言<paramref name="assertion"/>是否为真，如果不为真，抛出指定消息<paramref name="message"/>的指定类型<typeparamref name="TException"/>异常
        /// </summary>
        /// <typeparam name="TException">异常类型</typeparam>
        /// <param name="assertion">要验证的断言。</param>
        /// <param name="message">异常消息。</param>
        private static void Require<TException>(bool assertion, string message)
            where TException : Exception
        {
            if (assertion)
            {
                return;
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            TException exception = (TException)Activator.CreateInstance(typeof(TException), message);
            throw exception;
        }

        /// <summary>
        /// 检查参数不能为空引用，否则抛出<see cref="ArgumentNullException"/>异常。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull<T>(this T value, string paramName)
        {
            Require<ArgumentNullException>(value != null, $"参数“{paramName}”不能为空引用。");
        }

        /// <summary>
        /// 是否为Null
        /// </summary>
        /// <param name="value">判断的值</param>
        /// <returns>true为null,false不为null</returns>
        public static bool IsNull(this object value)
        {
            return value == null ? true : false;
        }
    }
}
