using AkliaJob.Models.Schedule;
using AkliaJob.SqlSugar.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.Repository.Schedule
{
    public class ScheduleRepository : SqlSugarRepository<ScheduleEntity>, IScheduleRepository
    {
        public ScheduleRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {

        }
    }
}
