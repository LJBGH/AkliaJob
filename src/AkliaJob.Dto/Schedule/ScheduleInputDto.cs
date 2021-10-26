using AkliaJob.Models.Schedule;
using AkliaJob.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AkliaJob.Dto.Schedule
{

    [AkliaAutoMapper(typeof(ScheduleEntity))]
    public class ScheduleInputDto
    {
        ///// <summary>
        ///// 主键(任务Id)
        ///// </summary>
        //[DisplayName("主键(任务Id)")]
        //public Guid Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DisplayName("任务名称")]
        public string JobName { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        [DisplayName("任务分组")]
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [DisplayName("任务状态")]
        public JobStatus JobStatus { get; set; }

        /// <summary>
        /// 任务运行状态
        /// </summary>
        [DisplayName("任务运行状态")]
        public RunStatus RunStatus { get; set; }

        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [DisplayName("任务运行时间表达式")]
        public string Cron { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [DisplayName("任务描述")]
        public string Remark { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [DisplayName("任务所在DLL对应的程序集名称")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 任务所在类
        /// </summary>                                                  0
        [DisplayName("任务所在类名称")]
        public string ClassName { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        [DisplayName("执行次数")]
        public int RunTimes { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName("开始时间")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName("结束时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextTime { get; set; }

        /// <summary>
        /// 触发器类型
        /// </summary>
        [DisplayName("触发器类型")]
        public TriggerType TriggerType { get; set; }

        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        [DisplayName("执行间隔时间, 秒为单位")]
        public int IntervalSecond { get; set; }


    }
}
