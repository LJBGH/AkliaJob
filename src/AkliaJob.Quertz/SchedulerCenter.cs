using AkliaJob.Models.Schedule;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using AkliaJob.Shared;
using AkliaJob.Quertz.Server;
using Quartz.Impl.Matchers;
using System.Collections.Generic;
using Quartz.Spi;
using Microsoft.Extensions.Logging;

namespace AkliaJob.Quertz
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    public class SchedulerCenter : IScheduleCenter
    {
        ///// <summary>
        ///// 任务调度对象
        ///// </summary>
        //public static readonly SchedulerCenter Instance;

        //static SchedulerCenter()
        //{
        //    Instance = new SchedulerCenter();
        //}



        private Task<IScheduler> _scheduler;

        private readonly IJobFactory _jobFactory;

        private readonly ILogger _logger;
        public SchedulerCenter(IJobFactory jobFactory, ILogger logger)
        {
            _scheduler = Scheduler;
            _jobFactory = jobFactory;
            _logger = logger;

        }


        /// <summary>
        /// 任务计划（调度器）
        /// </summary>
        /// <returns></returns>
        private Task<IScheduler> Scheduler
        {
            get
            {
                if (this._scheduler != null)
                {
                    return this._scheduler;
                }

                // 从Factory中获取Scheduler实例
                NameValueCollection props = new NameValueCollection
                {
                     { "quartz.serializer.type", "binary" }
                };

                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                return this._scheduler = factory.GetScheduler();
            }
        }

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<QuartzNetResult> StartScheduleAsync(List<ScheduleEntity> schedules)
        {
            try
            {
                this._scheduler.Result.JobFactory = this._jobFactory;
                if (!this._scheduler.Result.IsStarted) 
                {
                    //等待任务运行完成
                    await this._scheduler.Result.Start();
                }
                return new QuartzNetResult("任务调度开启成功");

                //await this._scheduler.Result.Start();
                //foreach (var item in schedules)
                //{
                //    ScheduleManage.Instance.AddScheduleList(item);
                //    await this.RunSchedule<ScheduleManage>(item.JobName, item.JobGroup);
                //}
            }
            catch (Exception ex)
            {
                return new QuartzNetResult("开启任务调度失败" + ex.Message, false); 
            }
        }


        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<QuartzNetResult> StopScheduleAsync()
        {
            try
            {
                //判断调度是否已经关闭
                if (!this._scheduler.Result.IsShutdown)
                {
                    await this._scheduler.Result.Standby();
                    return new QuartzNetResult("停止任务调度成功");
                }
                {
                    return new QuartzNetResult("任务调度已关闭，停止无效",false);
                }   
            }
            catch (Exception ex)
            {
                return new QuartzNetResult("停止任务调度失败" + ex.Message,false);
            }
        }


        /// <summary>
        /// 关闭任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<QuartzNetResult> CloseScheduleAsync()
        {
            try
            {
                //判断调度是否已经关闭
                if (!this._scheduler.Result.IsShutdown)
                {
                    await this.Scheduler.Result.Shutdown();
                    return new QuartzNetResult("关闭任务调度成功");
                }
                {
                    return new QuartzNetResult("任务调度已关闭，不可再次关闭",false);
                }
            }
            catch (Exception ex)
            {
                return new QuartzNetResult("关闭任务调度失败" + ex.Message, false);
            }
        }


        /// <summary>
        /// 运行指定的计划(映射处理IJob实现类)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<QuartzNetResult> RunSchedule<T>(string jobName, string jobGroup) where T : ScheduleManage, new()
        {
            QuartzNetResult result;
            try
            {
                //开启调度器
                await this._scheduler.Result.Start();

                //创建指定泛型类型参数指定的类型实例
                T t = Activator.CreateInstance<T>();
                //获取任务实例
                ScheduleEntity scheduleModel = t.GetScheduleModel(jobName, jobGroup);
                //添加任务
                var addResult = AddScheduleJob(scheduleModel).Result;
                if (addResult.Success == true)
                {
                    scheduleModel.JobStatus = JobStatus.Enabled;
                    t.UpdateScheduleStatus(scheduleModel);
                    await this._scheduler.Result.ResumeJob(new JobKey(jobName, jobGroup));
                    result = new QuartzNetResult("启动成功");
                }
                else
                {
                    result = new QuartzNetResult("启动失败", false);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            return result;
        }

        /// <summary>
        /// 添加一个工作调度(映射程序集指定IJob实现类)
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private async Task<QuartzNetResult> AddScheduleJob(ScheduleEntity schedule)
        {
            var result = new QuartzNetResult("");

            try
            {
                //检查任务是否已存在
                var jk = new JobKey(schedule.JobName, schedule.JobGroup);
                if (await this._scheduler.Result.CheckExists(jk))
                {
                    //删除已经存在任务
                    await this._scheduler.Result.DeleteJob(jk);
                }

                //反射获取任务执行类
                var jobType = AssemblyExtension.GetAssemblyClass("AkliaJob.TaskService", "AkliaJob.TaskService.TestJob");

                // 定义这个工作，并将其绑定到我们的IJob实现类
                IJobDetail job = new JobDetailImpl(schedule.JobName, schedule.JobGroup, jobType);

                // 创建触发器
                ITrigger trigger;
                //校验Cron表达式是否正确
                if (!string.IsNullOrEmpty(schedule.Cron) && CronExpression.IsValidExpression(schedule.Cron))
                {
                    trigger = CreateCronTrigger(schedule);
                }
                else
                {
                    trigger = CreateSimpleTrigger(schedule);
                }
                // 设置监听器
                JobListener listener = new JobListener();
                this._scheduler.Result.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());

                // 告诉调度器使用触发器来安排作业
                await this._scheduler.Result.ScheduleJob(job, trigger);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Msg = ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 暂停任务计划
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        public async Task<QuartzNetResult> StopScheduleJob<T>(string jobName, string jobGroup, bool isDelete = false) where T : ScheduleManage, new()
        {
            var result = new QuartzNetResult("");
            try
            {
                //检查任务是否已存在
                var jk = new JobKey(jobName, jobGroup);
                if (!await this._scheduler.Result.CheckExists(jk))
                {
                    return new QuartzNetResult("任务未运行", false);
                }
                //暂停任务
                await this._scheduler.Result.PauseJob(jk);

                //是否移除任务
                if (isDelete)
                {
                    Activator.CreateInstance<T>().RemoveScheduleModel(jobGroup, jobName);
                }
                result = new QuartzNetResult ("停止任务成功");

            }
            catch (Exception ex)
            {
                result = new QuartzNetResult("任务停止失败" + ex.Message, false);
            }
            return result;
        }

        /// <summary>
        /// 恢复运行暂停的任务(暂停之后继续运行)
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="JobGroup"></param>
        /// <returns></returns>
        public async Task<QuartzNetResult> ResumeJob(string jobName, string jobGroup) 
        {
            var result = new QuartzNetResult("");
            try
            {
                //检查任务是否已存在
                var jk = new JobKey(jobName, jobGroup);
                if (!await this._scheduler.Result.CheckExists(jk))
                {
                    result = new QuartzNetResult("任务未运行",false);
                }
                //恢复运行暂停任务
                await this._scheduler.Result.ResumeJob(jk);
                result = new QuartzNetResult("恢复运行任务成功");
            }
            catch (Exception ex) 
            {
                result = new QuartzNetResult("恢复运行任务失败" + ex.Message, false);
            }
            return result;       
        }


        /// <summary>
        /// 创建自定义类型的触发器
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(ScheduleEntity schedule)
        {
            if (schedule.RunTimes > 0)
            {
                return TriggerBuilder.Create()
                        .WithIdentity(schedule.JobName, schedule.JobGroup)
                        .StartAt(schedule.BeginTime)//开始时间
                        .EndAt(schedule.EndTime)//结束数据
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(schedule.IntervalSecond)//执行时间间隔，单位秒
                            .WithRepeatCount(schedule.RunTimes))//执行次数、默认从0开始
                            .ForJob(schedule.JobName, schedule.JobGroup)//作业名称
                        .Build();
            }
            else 
            {
                return TriggerBuilder.Create()
                     .WithIdentity(schedule.JobName, schedule.JobGroup)
                     .StartAt(schedule.BeginTime)//开始时间
                     .EndAt(schedule.EndTime)//结束时间
                     .WithSimpleSchedule(x => x
                         .WithIntervalInSeconds(schedule.IntervalSecond)//执行时间间隔，单位秒
                         .RepeatForever())   //无线循环
                         .ForJob(schedule.JobName, schedule.JobGroup)//作业名称
                     .Build();
            }
        }




        /// <summary>
        /// 创建Cron类型的触发器
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(ScheduleEntity schedule)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(schedule.JobName, schedule.JobGroup)
                   .StartAt(schedule.BeginTime)//开始时间
                   .EndAt(schedule.EndTime)//结束数据
                   .WithCronSchedule(schedule.Cron)//指定cron表达式
                   .ForJob(schedule.JobName, schedule.JobGroup)//作业名称
                   .Build();
        }

    }
}
