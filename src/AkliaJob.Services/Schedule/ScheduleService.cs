using AkliaJob.Models.Schedule;
using AkliaJob.Repository.Schedule;
using AkliaJob.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.Services.Schedule
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        /// <summary>
        /// 添加一条任务计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AjaxResult> InsertAsync(ScheduleEntity entity)
        {
            return await _scheduleRepository.InsertAsync(entity);
        }
    }
}
