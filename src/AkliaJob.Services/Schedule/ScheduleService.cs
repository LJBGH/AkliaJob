using AkliaJob.Models.Schedule;
using AkliaJob.Quertz;
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
        /// 获取所有计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScheduleEntity>> GetAllASync()
        {
            return await _scheduleRepository.GetAsync();
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

            ScheduleManage.Instance.AddScheduleList(scheduleEntity);

            QuartzNetResult result;
            if (scheduleEntity.TriggerType == TriggerType.Simple)
            {
                result = await SchedulerCenter.Instance.RunSchedule<ScheduleManage>(scheduleEntity.JobGroup, scheduleEntity.JobName);
                //result = SchedulerCenter.Instance.RunScheduleJob<>
            }
            else 
            {
                result = await SchedulerCenter.Instance.RunSchedule<ScheduleManage>(scheduleEntity.JobGroup, scheduleEntity.JobName);
            }
            return new AjaxResult(result.Success == true ? "执行成功" : "执行失败", result.Success == true ? AjaxResultType.Success : AjaxResultType.Error);
        }

    }

}
