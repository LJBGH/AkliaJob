using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.Shared
{
    /// <summary>
    /// 公共拓展模块注入
    /// </summary>
    public static class CommonExtendModule
    {
        public static void AddCommonModule(this IServiceCollection service) 
        {
            service.AddHttpContextAccessor();
            service.AddSingleton<IAkliaUser, AkliaUser>();
        }

    }
}
