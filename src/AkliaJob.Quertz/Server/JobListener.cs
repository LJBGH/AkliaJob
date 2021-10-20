using Microsoft.Extensions.Logging;
using Quartz;
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
        public string Name => "JobListener";
        public static int count = 0;


        //job开始之前调用
        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            Console.WriteLine("job开始之前调用");
        }

        //job每次执行之后调用
        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            Console.WriteLine("job每次执行之后调用");
        }

        //job执行结束之后调用
        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            count++;
            var manage = new ScheduleManage();
            var model = manage.GetScheduleModel(context.JobDetail.Key.Group, context.JobDetail.Key.Name);
            Console.WriteLine(model.JobName + "job执行结束之后调用  " + count);
        }
    }
}
