using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AkliaJob.Center.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //添加autofac服务,将内置容器替换为autofac容器
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })

            //注入Serilog日志中间件
            .UseSerilog((webHost, configuration) =>
            {
                //得到配置文件
                var serilog = webHost.Configuration.GetSection("Serilog");
                //最小级别
                var minimumLevel = serilog["MinimumLevel:Default"];
                //日志事件级别
                var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minimumLevel);

                configuration.ReadFrom
                .Configuration(webHost.Configuration.GetSection("Serilog"))
                .Enrich.FromLogContext()//使用Serilog.Context.LogContext中的属性丰富日志事件
                .WriteTo.Console(logEventLevel);//输出到控制台

                //写入本地txt
                configuration.WriteTo.Map(le => MapData(le), (key, log) =>
                 log.Async(o => o.File(Path.Combine("Logs", @$"{key.time:yyyy-MM-dd}\{key.level.ToString().ToLower()}.txt"), logEventLevel)));

                (DateTime time, LogEventLevel level) MapData(LogEvent logEvent)
                {
                    return (new DateTime(logEvent.Timestamp.Year, logEvent.Timestamp.Month, logEvent.Timestamp.Day, logEvent.Timestamp.Hour, logEvent.Timestamp.Minute, logEvent.Timestamp.Second), logEvent.Level);
                }
            });
    }
}
