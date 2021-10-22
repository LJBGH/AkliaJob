using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.App.Extensions
{
    public static class NlogExtension
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="services"></param>
        public static void AddNLog(this IServiceCollection services) 
        {
            // 从appsettings.json文件中读入日志的配置信息
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                loggingBuilder.AddConsole(); // 将日志输出到控制台
            });
        }
    }
}
