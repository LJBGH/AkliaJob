﻿using AkliaJob.Shared;
using SqlSugar;
using System;
using System.ComponentModel;

namespace AkliaJob.Models.Schedule
{
    public class SchedueEntity : IFullAuditedEntity
    {
        /// <summary>
        /// 主键(任务Id)
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [DisplayName("主键(任务Id)")]
        public int Id { get; set; }

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
        [DisplayName("状态")]
        public JobStatus JobStatus { get; set; }

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
        /// 触发器类型
        /// </summary>
        [DisplayName("触发器类型")]
        public TriggerType TriggerType { get; set; }

        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        [DisplayName("执行间隔时间")]
        public int IntervalSecond { get; set; }



        ///// <summary>
        ///// 任务所在DLL对应的程序集名称
        ///// </summary>
        //public string AssemblyName { get; set; }    

        ///// <summary>
        ///// 任务所在类
        ///// </summary>
        //public string ClassName { get; set; }


        #region   通用字段
        /// <summary>
        /// 创建人Id
        /// </summary>
        [DisplayName("创建人Id")]
        public Guid CreatedId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最后修改人Id
        /// </summary>
        [DisplayName("最后修改人Id")]
        public Guid? LastModifyId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DisplayName("最后修改时间")]
        public DateTime LastModifedAt { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [DisplayName("是否删除")]
        public bool IsDeleted { get; set; }
        #endregion
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum JobStatus 
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Runing = 0,

        /// <summary>
        /// 已暂停
        /// </summary>
        Suspended = 1
    }

    /// <summary>
    /// 触发器类型
    /// </summary>
    public enum TriggerType 
    {
        /// <summary>
        /// 简单类型触发器
        /// </summary>
        Simple = 0,

        /// <summary>
        /// Cron表达式
        /// </summary>
        Cron = 1
    }
}