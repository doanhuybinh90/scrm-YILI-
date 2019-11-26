using Newtonsoft.Json;
using Pb.Hangfire.Tool;
using Pb.Wechat.MpAccounts.Dto;
using Pb.Wechat.MpCourseSignups.Dto;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class WeChatSendCourseMessage : IJob
    {
        public void Run()
        {
            Task.WaitAll(DoRun());
        }
        public async Task DoRun()
        {
            IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);

            var accounts = await db.FindToListAsync<MpAccountDto>("SELECT * FROM MpAccounts WHERE ISDELETED=0 ", null, false);
            var sendList = await db.FindToListAsync<MpCourseSignupDto>("SELECT * FROM MpCourseSignups WHERE ISDELETED=0 AND DATEDIFF(HOUR,GETDATE(),BEGINTIME)<=2 and SendMessageState=0", null, false);
            var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
            var url = JobConfig.AppSettings["SendTemplateMessageUrl"];
            var templateId = JobConfig.AppSettings["TemplateID"];
            var coursedetialurl = JobConfig.AppSettings["WKTCourseDetailUrl"];
            using (HttpClient httpclient = new HttpClient(handler))
            {
                httpclient.BaseAddress = new Uri(url);

                foreach (var item in sendList)
                {
                    var tempdata = new {
                        touser= item.OpenID,
                        template_id= templateId,
                        url= coursedetialurl,
                        data = new
                        {
                            first = new { value = "亲爱的会员您好，您报名的课程即将开始" },//标题
                            keyword1 = new { value = item.CourseName },//课程名称
                            keyword2 = new { value = item.BeginTime },//课程时间
                            keyword3 = new { value = item.Address },//上课地点
                            keyword4 = new { value = "400-800-1111" },//联系方式
                            remark = new { value = item.Reamrk }//备注
                        }
                    };

                    var token = accounts.Where(m => m.Id == item.MpID).FirstOrDefault().TaskAccessToken;
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>() {{ "data", JsonConvert.SerializeObject(tempdata) }});
                    var response = await httpclient.PostAsync(url, content);

                    string responseString = await response.Content.ReadAsStringAsync();
                    await db.ExcuteNonQueryAsync("Update MpCourseSignups Set SendTime=GetDate(),SendMessageState=1,SendResult='" + responseString.Replace("'","''") + "' WHERE ID=" + item.Id, null, false);

                }
            }

        }
    }
}
