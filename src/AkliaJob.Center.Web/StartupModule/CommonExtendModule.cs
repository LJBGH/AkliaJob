using AkliaJob.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AkliaJob.Swagger;
using Microsoft.AspNetCore.Builder;

namespace AkliaJob.Center.Web.StartupModule
{
    /// <summary>
    /// 公共拓展模块注入
    /// </summary>
    public static class CommonExtendModule
    {
        public static void AddCommonService(this IServiceCollection service) 
        {
            //swagger注入
            service.AddSwaggerService();

            service.AddHttpContextAccessor();
            service.AddSingleton<IAkliaUser, AkliaUser>();
        }

        public static IApplicationBuilder UseCommonExtension(this IApplicationBuilder app) 
        {
            app.UseSwaggerService();
            return app;
        }

    }
}
