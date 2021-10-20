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
    [OpenApiTag("1.3.1 医院管理", Description = "HospitalController")]
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
        public async Task<AjaxResult> InsertAssync(ScheduleEntity entity) 
        {
            return await _scheduleService.InsertAsync(entity);
        }
    }
}
