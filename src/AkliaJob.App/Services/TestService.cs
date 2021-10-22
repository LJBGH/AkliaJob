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
        private readonly ILogger<TestService> _logger;

        public TestService(IScheduleRepository scheduleRepository, ILogger<TestService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;
        }

        public async Task TestAsync()
        {
            await Task.CompletedTask;
            _logger.LogInformation("服务注入测试成功");
        }
    }
}
