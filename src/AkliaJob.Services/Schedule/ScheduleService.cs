using AkliaJob.Models.Schedule;
using AkliaJob.Repository.Schedule;
using AkliaJob.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.Services.Schedule
{
    /// <summary>
    /// 任务计划管理
    /// </summary>
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


        /// <summary>
        /// 执行一条任务计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AjaxResult> ExecuteAsync(Guid id)
        {
            var scheduleEntity = await _scheduleRepository.GetByIdAsync(id);
            if (scheduleEntity == null)
                return new AjaxResult("未找到该任务", AjaxResultType.Success);
            
            throw new NotImplementedException();
        }
    }

}
