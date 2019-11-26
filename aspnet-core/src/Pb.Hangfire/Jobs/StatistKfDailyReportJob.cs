using Pb.Hangfire.Tool;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceOnlines.Dto;
using Pb.Wechat.MpAccounts.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Pb.Wechat.CustomerServiceReports.Dto;
using System.ComponentModel;

namespace Pb.Hangfire.Jobs
{
    class StatistKfDailyReportJob : IJob
    {
        public void Run()
        {
            Task.WaitAll(StatistDatas());
        }
        //[AutomaticRetry(Attempts = 1)]

        public async Task StatistDatas()
        {
            IDBHelper defaultdb = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);
            var accounts = await defaultdb.FindToListAsync<MpAccountDto>("SELECT * FROM MpAccounts WHERE ISDELETED=0 ", null, false);
            foreach (var account in accounts)
            {
                IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["Kf"]);
                var now = DateTime.Now;
                var startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var endTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                string msgSql = $"SELECT * FROM CustomerServiceConversationMsgs WHERE CreationTime>='{startTime}' AND CreationTime<='{endTime}' AND Sender={(int)CustomerServiceMsgSender.customer} AND MpID={account.Id}";
                var msgList = await db.FindToListAsync<CustomerServiceConversationMsgDto>(msgSql, null, false);


                string conSql = $"SELECT * FROM CustomerServiceConversations WHERE MpID={account.Id} AND CreationTime>='{startTime}' AND CreationTime<='{endTime}'";
                var conList = await db.FindToListAsync<CustomerServiceConversationDto>(conSql, null, false);

                string inoutLogSql = $"SELECT * FROM CustomerInOutLogs WHERE CreationTime>='{startTime}' AND CreationTime<='{endTime}'";
                var inoutList = await db.FindToListAsync<LogInout>(inoutLogSql, null, false);

                var customers = await db.FindToListAsync<CustomerServiceOnlineDto>($"SELECT * FROM dbo.CustomerServiceOnlines WHERE KfType='{KFType.YL.ToString()}' AND IsDeleted=0 AND MpID={account.Id}", null, false);

                foreach(var customer in customers)
                {
                    var cusMsgList = msgList.Where(m => m.CustomerId == customer.Id).ToList();
                    var cusConList = conList.Where(m => m.CustomerId == customer.Id).ToList();
                    var cusInoutList = inoutList.Where(m => m.CustomerId == customer.Id).OrderBy(m=>m.CreationTime).ToList();

                    CustomerServiceReportDto model = new CustomerServiceReportDto();
                    model.ServiceMsgCount = cusMsgList.Count();
                    model.ServiceCount = cusConList.Count();
                    model.ReceiveCount = cusConList.Select(m => m.FanId).Distinct().Count();
                    var source = cusConList.Where(m => m.ConversationScore > -1).ToList();
                    if (source.Count > 0)
                        model.AvgScore = Math.Round(Convert.ToDecimal(source.Average(m => m.ConversationScore)),2);
                    model.MpID = account.Id;
                    model.CreationTime = DateTime.Now;
                    model.NickName = customer.KfNick;
                    model.ReportDate = startTime;
                    model.CustomerId = customer.Id;
                    model.ScoreCount = source.Count();
                    model.TotalScore = source.Sum(m => m.ConversationScore);
                    #region onlineTime
                    var t = 0;
                    var start = false;
                    var minutes = 0.0;
                    DateTime? _sD = null;
                    DateTime? _eD = null;
                    for (int i = 0; i < cusInoutList.Count; i++)
                    {
                        if (!start)
                        {
                            if (cusInoutList[i].InOutState == (int)InOutState.Login || cusInoutList[i].InOutState == (int)InOutState.online || cusInoutList[i].InOutState == (int)InOutState.ReLogin)
                            {
                                start = true;
                                _sD = cusInoutList[i].CreationTime;
                            }
                            else
                                continue;
                        }
                        else
                        {
                            if (_sD.HasValue)
                            {
                                if (cusInoutList[i].InOutState == (int)InOutState.Logout || cusInoutList[i].InOutState == (int)InOutState.leave || cusInoutList[i].InOutState == (int)InOutState.quit)
                                {
                                    _eD = cusInoutList[i].CreationTime;
                                    TimeSpan sp = _eD.Value.Subtract(_sD.Value);
                                    minutes += sp.TotalMinutes;
                                    _sD = null;
                                    _eD = null;
                                }
                                else
                                    continue;
                            }
                            else
                            {
                                if (cusInoutList[i].InOutState == (int)InOutState.Login || cusInoutList[i].InOutState == (int)InOutState.online || cusInoutList[i].InOutState == (int)InOutState.ReLogin)
                                {
                                    _sD = cusInoutList[i].CreationTime;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    model.OnlineTime = Math.Round(Convert.ToDecimal(minutes) / 60, 2);

                    #endregion
                    var sql = model.GetInsertSql("CustomerServiceReports", "Id");
                    await db.ExcuteNonQueryAsync(sql, null, false);

                    

                }


            }

        }
    }

    public class LogInout
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public int InOutState { get; set; }
        public DateTime CreationTime { get; set; }
    }
    public enum InOutState
    {
        [Description("登入")]
        Login = 1,
        [Description("登出")]
        Logout = 2,
        [Description("断线重连")]
        ReLogin = 3,
        online = 4,
        leave = 5,
        quit = 6
    }
}
