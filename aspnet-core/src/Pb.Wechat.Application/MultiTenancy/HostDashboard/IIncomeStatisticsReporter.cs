using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pb.Wechat.MultiTenancy.HostDashboard.Dto;

namespace Pb.Wechat.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}