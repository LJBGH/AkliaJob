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
using AkliaJob.Quertz.Jobs;
using AkliaJob.AutoMapper;
using AkliaJob.Consul;
using AkliaJob.SqlSugar.Repository;

namespace AkliaJob.Center.Web.StartupModule
{
  
    public static class CommonExtendModule
    {
        /// <summary>
        /// 公共拓展模块注入
        /// </summary>
        public static void AddCommonService(this IServiceCollection service) 
        {
            //swagger注入
            service.AddSwaggerService();


            service.AddHttpContextAccessor();
            service.AddSingleton<IAkliaUser, AkliaUser>();

            service.AddScoped(typeof(ISqlSugarRepository<>),typeof(SqlSugarRepository<>));

            //AutoMapper注入
            service.AddAutoMapperService();

            //调度中心注入
            service.AddSingleton<IScheduleCenter, SchedulerCenter>();



            //Job定时任务注入
            service.AddJobService();

        }



        /// <summary>
        /// Job定时任务注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddJobService(this IServiceCollection services) 
        {
            services.AddSingleton<IJobFactory, IOCJobFactory>();
            List<Type> jobtype = new List<Type>();
            jobtype.Add(typeof(TestJob));
            foreach (var item in jobtype)
            {
                services.AddSingleton(item);//Job使用瞬时依赖注入
            }
        }




        //公共中间件
        public static IApplicationBuilder UseCommonExtension(this IApplicationBuilder app) 
        {
            //Consul配置
            //app.UseConsul();

            //Swagger配置
            app.UseSwaggerService();

            //开启定时任务
            //app.StartJob();
            return app;
        }


        //开启定时任务
        public static void StartJob(this IApplicationBuilder app)
        {
            var scheduleService = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IScheduleService>();

            scheduleService.StartScheduleAsync();
        }

    }
}
