using AkliaJob.Models.Schedule;
using AkliaJob.SqlSugar.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.Repository.Schedule
{
    public interface IScheduleRepository :ISqlSugarRepository<ScheduleEntity>
    {

    }
}
