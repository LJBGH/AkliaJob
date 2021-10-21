using AkliaJob.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AkliaJob.Quertz
{
    public interface IScheduleCenter
    {

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<QuartzNetResult> StartScheduleAsync(List<ScheduleEntity> schedules);

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<QuartzNetResult> StopScheduleAsync();

        /// <summary>
        /// 关闭任务调度
        /// </summary>
        /// <returns></returns>
        Task<QuartzNetResult> CloseScheduleAsync();

        /// <summary>
        /// 运行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        /// <returns></returns>
        Task<QuartzNetResult> RunSchedule<T>(string jobName, string jobGroup) where T : ScheduleManage, new();


        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        Task<QuartzNetResult> StopScheduleJob<T>(string jobName, string jobGroup, bool isDelete = false) where T : ScheduleManage, new();


        /// <summary>
        /// 恢复任暂停的任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        /// <returns></returns>
        Task<QuartzNetResult> ResumeJob(string jobName, string jobGroup);


    }
}
