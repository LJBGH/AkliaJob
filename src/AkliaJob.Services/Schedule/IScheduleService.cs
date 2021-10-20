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

        /// <summary>
        /// 添加一条任务计划 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AjaxResult> InsertAsync(ScheduleEntity entity);

        /// <summary>
        /// 执行任务计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AjaxResult> ExecuteAsync(Guid id);

    }
}
