using AkliaJob.Models.Schedule;
using AkliaJob.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.Services.Schedule
{
    public interface IScheduleService
    {
        Task<List<ScheduleEntity>> GetAllASync();


        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        Task<AjaxResult> StartScheduleAsync();

        /// <summary>
        /// 关闭任务调度
        /// </summary>
        /// <returns></returns>
        Task<AjaxResult> StopScheduleAsync();

        /// <summary>
        /// 添加一条任务计划 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AjaxResult> InsertAsync(ScheduleEntity entity);

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AjaxResult> ExecuteAsync(Guid id);

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AjaxResult> StopAsync(Guid id);

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AjaxResult> ResumeAsync(Guid id);

    }
}
