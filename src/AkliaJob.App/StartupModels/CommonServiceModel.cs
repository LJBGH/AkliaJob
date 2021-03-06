using AkliaJob.App.Services;
using AkliaJob.Repository.Schedule;
using AkliaJob.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.App.StartupModels
{
    public static class CommonServiceModel
    {
        /// <summary>
        /// 自定义模块构建
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider ServicesBilder(this IServiceCollection services) 
        {
            services.AddLogging();

            services.AddHttpContextAccessor();
            services.AddSingleton<IAkliaUser, AkliaUser>();

            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ITestService, TestService>();

            return services.BuildServiceProvider();
        }
    }
}
