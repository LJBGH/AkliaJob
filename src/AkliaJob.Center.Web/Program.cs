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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //���autofac����,�����������滻Ϊautofac����
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })

            //ע��Serilog��־�м��
            .UseSerilog((webHost, configuration) =>
            {
                //�õ������ļ�
                var serilog = webHost.Configuration.GetSection("Serilog");
                //��С����
                var minimumLevel = serilog["MinimumLevel:Default"];
                //��־�¼�����
                var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minimumLevel);

                configuration.ReadFrom
                .Configuration(webHost.Configuration.GetSection("Serilog"))
                .Enrich.FromLogContext()//ʹ��Serilog.Context.LogContext�е����Էḻ��־�¼�
                .WriteTo.Console(logEventLevel);//���������̨

                //д�뱾��txt
                configuration.WriteTo.Map(le => MapData(le), (key, log) =>
                 log.Async(o => o.File(Path.Combine("Logs", @$"{key.time:yyyy-MM-dd}\{key.level.ToString().ToLower()}.txt"), logEventLevel)));

                (DateTime time, LogEventLevel level) MapData(LogEvent logEvent)
                {
                    return (new DateTime(logEvent.Timestamp.Year, logEvent.Timestamp.Month, logEvent.Timestamp.Day, logEvent.Timestamp.Hour, logEvent.Timestamp.Minute, logEvent.Timestamp.Second), logEvent.Level);
                }
            });
    }
}
