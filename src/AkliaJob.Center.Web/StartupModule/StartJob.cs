using AkliaJob.Models.Schedule;
using AkliaJob.Quertz;
using AkliaJob.Services.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkliaJob.Center.Web.StartupModule
{
    public class StartJob
    {

        private readonly IScheduleService _scheduleService;

        public StartJob(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task StartJobAsync() 
        {
            var scheduleList = await _scheduleService.GetAllASync();
            var List = scheduleList.Where(x => x.JobStatus == JobStatus.Enabled).ToList();
            foreach (var item in List)
            {
                ScheduleManage.Instance.AddScheduleList(item);
                await SchedulerCenter.Instance.RunSchedule<ScheduleManage>(item.JobGroup, item.JobName);
            }

        }
    }
}
