using AkliaJob.Dto.Schedule;
using AkliaJob.Models.Schedule;
using AkliaJob.Quertz;
using AkliaJob.Repository.Schedule;
using AkliaJob.Shared;
using AkliaJob.SqlSugar.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IScheduleCenter _scheduleCenter;

        private readonly ISqlSugarRepository<ScheduleEntity> _sqlSugarRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, IScheduleCenter scheduleCenter, ISqlSugarRepository<ScheduleEntity> sqlSugarRepository)
        {

            _sqlSugarRepository = sqlSugarRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleCenter = scheduleCenter;
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
        /// 开启任务调度
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public async Task<AjaxResult> StartScheduleAsync()
        {
            var schedules = await GetAllASync();
            var list = schedules.Where(x => x.JobStatus == JobStatus.Enabled).ToList();
            var result = await _scheduleCenter.StartScheduleAsync(list);

            return new AjaxResult(result.Success == true ? "开启任务调度成功" : "开启任务调度失败", result.Success == true ? AjaxResultType.Success : AjaxResultType.Error);
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public async Task<AjaxResult> StopScheduleAsync()
        {
            var result = await _scheduleCenter.StopScheduleAsync();

            return new AjaxResult(result.Success == true ? "停止任务调度成功" : "停止任务调度失败", result.Success == true ? AjaxResultType.Success : AjaxResultType.Error);
        }

        /// <summary>
        /// 添加一条任务计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AjaxResult> InsertAsync(ScheduleInputDto inputDto)
        {
           

            var entity = inputDto.MapTo<ScheduleEntity>();
            return await _sqlSugarRepository.InsertAsync(entity);
            //return await _scheduleRepository.InsertAsync(entity);
        }


        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AjaxResult> ExecuteAsync(Guid id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                return new AjaxResult("未找到该任务", AjaxResultType.Success);

            ScheduleManage.Instance.AddScheduleList(schedule);

            QuartzNetResult result;
            if (schedule.TriggerType == TriggerType.Simple)
            {
                result = await _scheduleCenter.RunSchedule<ScheduleManage>(schedule.JobName, schedule.JobGroup);
                //result = SchedulerCenter.Instance.RunScheduleJob<>
            }
            else 
            {
                result = await _scheduleCenter.RunSchedule<ScheduleManage>(schedule.JobName, schedule.JobGroup);
            }

            if (result.Success == true) 
            {
                schedule.JobStatus = JobStatus.Enabled;
                await _scheduleRepository.UpdateAsync(schedule);
            }
            return new AjaxResult(result.Success == true ? "执行成功" : "执行失败", result.Success == true ? AjaxResultType.Success : AjaxResultType.Error);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AjaxResult> StopAsync(Guid id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                return new AjaxResult("未找到该任务", AjaxResultType.Success);

            var result = await _scheduleCenter.StopScheduleJob<ScheduleManage>(schedule.JobName, schedule.JobGroup);
            if (result.Success == true) 
            {
                schedule.JobStatus = JobStatus.Stoped;
                await _scheduleRepository.UpdateAsync(schedule);
            }
            return new AjaxResult(result.Success == true ? "停止任务成功" : "停止任务失败", result.Success == true ? AjaxResultType.Success : AjaxResultType.Error);
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AjaxResult> ResumeAsync(Guid id)
        {
            var scheduleEntity = await _scheduleRepository.GetByIdAsync(id);
            if (scheduleEntity == null)
                return new AjaxResult("未找到该任务", AjaxResultType.Success);

            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                return new AjaxResult("未找到该任务", AjaxResultType.Success);

            var result = await _scheduleCenter.ResumeJob(schedule.JobName, schedule.JobGroup);

            if (result.Success == true)
            {
                schedule.JobStatus = JobStatus.Enabled;
                await _scheduleRepository.UpdateAsync(schedule);
            }
            return new AjaxResult(result.Success == true ? "恢复任务成功" : "恢复任务失败", result.Success == true ? AjaxResultType.Success : AjaxResultType.Error);
        }

        /// <summary>
        /// 获取所有计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<AjaxResult> GetAllAsync()
        {
            var list = await _scheduleRepository.GetAsync();
            var result = list.MapToList<ScheduleOutDto>();

            return new AjaxResult(ResultMessage.LoadSucces, result, AjaxResultType.Success);
        }
    }
}
