
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkliaJob.Quertz.Server
{
    /// <summary>
    /// Job监听器
    /// </summary>
    public class JobListener : IJobListener
    {
        private readonly ILogger _logger;


        public string Name => "JobListener";
        public static int count = 0;

        public JobListener(ILogger logger)
        {
            _logger = logger;
        }


        //job开始之前调用
        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            //Console.WriteLine("job开始之前调用");
            _logger.Information("job开始之前调用");
        }

        //job每次执行之后调用
        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            //Console.WriteLine("job每次执行之后调用");
            _logger.Information("job每次执行之后调用");
        }

        //job执行结束之后调用
        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            count++;
            var manage = new ScheduleManage();
            var model = manage.GetScheduleModel(context.JobDetail.Key.Name, context.JobDetail.Key.Group);
            //Console.WriteLine(model.JobName + "job执行结束之后调用  " + count);
            _logger.Information(model.JobName + "job执行结束之后调用  " + count);
        }
    }
}
