using AkliaJob.Repository.Schedule;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AkliaJob.TaskService
{
    public class TestJob : IJob
    {
        int count = 0;
        //private readonly IScheduleRepository _scheduleRepository;

        //public TestJob(IScheduleRepository scheduleRepository)
        //{
        //    _scheduleRepository = scheduleRepository;
        //}

        public async Task Execute(IJobExecutionContext context)
        {

            count++;

            await Task.CompletedTask;
            Console.WriteLine("测试Job开始" + count);
        }
    }
}
