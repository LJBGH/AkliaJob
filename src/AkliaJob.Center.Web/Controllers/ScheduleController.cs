using AkliaJob.Dto.Schedule;
using AkliaJob.Models.Schedule;
using AkliaJob.Services.Schedule;
using AkliaJob.Shared;
using AkliaJob.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AkliaJob.Center.Web.Controllers
{
    /// <summary>
    /// 任务计划管理
    /// </summary>
    [Description("任务计划管理")]
    public class ScheduleController : ApiControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private ILogger _logger;
       

        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("开启任务调度")]
        public async Task<AjaxResult> StartScheduleAsync() 
        {
            return await _scheduleService.StartScheduleAsync();
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("停止任务调度")]
        public async Task<AjaxResult> StopScheduleAsync() 
        {
            return await _scheduleService.StopScheduleAsync();
        }


        /// <summary>
        /// 添加一条任务计划
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [DisplayName("添加一条任务计划")]
        public async Task<AjaxResult> InsertAsync(ScheduleInputDto inputDto) 
        {
            return await _scheduleService.InsertAsync(inputDto);
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("执行任务")]
        public async Task<AjaxResult> ExecuteAsync(Guid id) 
        {
            return await _scheduleService.ExecuteAsync(id);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("停止任务")]
        public async Task<AjaxResult> StopAsync(Guid id) 
        {
            return await _scheduleService.StopAsync(id);
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("恢复任务")]
        public async Task<AjaxResult> ResumeAsync(Guid id)
        {
            return await _scheduleService.ResumeAsync(id);
        }

        /// <summary>
        /// 获取有所计划任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("获取有所计划任务")]
        public async Task<AjaxResult> GetAllAsync() 
        {
            return await _scheduleService.GetAllAsync();
        }

    }
}
