using AkliaJob.Models.Schedule;
using AkliaJob.Repository.Schedule;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.App.Services
{
    public class TestService : ITestService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ILogger _logger;

        public TestService(IScheduleRepository scheduleRepository,ILogger<TestService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;
            //_logger = loggerFactory.CreateLogger<TestService>();

        }

        public async Task TestAsync()
        {
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

            var data = await _scheduleRepository.InsertNoCheckAsync(schedule);
            if (data.Success) 
            {
                _logger.LogInformation("插入数据测试成功");
            }
        }
    }
}
