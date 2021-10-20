using AkliaJob.Models.Schedule;
using AkliaJob.Services.Schedule;
using AkliaJob.Shared;
using AkliaJob.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
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

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }


        /// <summary>
        /// Test测试
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("Test测试")]
        public async Task<AjaxResult> Test()
        {
            await Task.CompletedTask;
            throw new Exception("Error ");
        }

        /// <summary>
        /// 添加一条任务计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [DisplayName("添加一条任务计划")]
        public async Task<AjaxResult> InsertAsync(ScheduleEntity entity) 
        {
            return await _scheduleService.InsertAsync(entity);
        }

        /// <summary>
        /// 执行一条任务计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [DisplayName("执行一条任务计划")]
        public async Task<AjaxResult> ExecuteAsync(Guid id) 
        {
            return await _scheduleService.ExecuteAsync(id);
        }
    }
}
