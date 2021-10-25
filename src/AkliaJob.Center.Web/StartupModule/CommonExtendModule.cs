using AkliaJob.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AkliaJob.Swagger;
using Microsoft.AspNetCore.Builder;
using AkliaJob.Services.Schedule;
using System.Threading;
using AkliaJob.Quertz;
using Quartz.Spi;
using AkliaJob.TaskService;

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

            //调度中心注入
            service.AddSingleton<IScheduleCenter, SchedulerCenter>();
            service.AddSingleton<IJobFactory, IOCJobFactory>();
            service.AddTransient(typeof(TestJob));
           
        }

        public static IApplicationBuilder UseCommonExtension(this IApplicationBuilder app) 
        {
            app.UseSwaggerService();

            //开启定时任务
            //app.StartJob();
            return app;
        }


        public static void StartJob(this IApplicationBuilder app)
        {
            var scheduleService = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IScheduleService>();

            scheduleService.StartScheduleAsync();
        }

    }
}
