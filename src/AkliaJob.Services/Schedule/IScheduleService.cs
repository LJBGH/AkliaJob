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

        Task<AjaxResult> InsertAsync(ScheduleEntity entity);
    }
}
