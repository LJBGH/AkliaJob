using AkliaJob.Models.Schedule;
using AkliaJob.Repository.Schedule;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AkliaJob.Quertz.Jobs
{
    public class TestJob : IJob
    {
        int count = 0;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ILogger _logger;

        public TestJob(IScheduleRepository scheduleRepository, ILogger<TestJob> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            count++;
            await Task.CompletedTask;
            var schedule = new ScheduleEntity
            {
                Id = Guid.NewGuid(),
                JobName = "测试",
                CreatedAt = DateTime.Now,
                CreatedId = Guid.NewGuid(),
                LastModifedAt = DateTime.Now,
                LastModifyId = Guid.NewGuid()

            };
            await _scheduleRepository.InsertNoCheckAsync(schedule);
            //Console.WriteLine("测试Job开始" + count);
            _logger.LogInformation("测试Job开始" + count);
        }
    }
}
