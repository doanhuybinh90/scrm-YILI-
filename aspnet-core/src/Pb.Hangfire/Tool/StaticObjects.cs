using Newtonsoft.Json;
using Pb.Hangfire.Jobs;
using Pb.Wechat.MpMessages.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pb.Hangfire.Tool
{
    public static class StaticObjects
    {
        public static IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["TestDb"]);

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public static async Task<ApiTokenResult> GetAccessToken(string token, int getnewtoken = 0)
        {
            ApiTokenResult accessTokenResult = null;
            var url = JobConfig.AppSettings["ApiTokenUrl"];
            var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
            using (HttpClient httpclient = new HttpClient(handler))
            {
                httpclient.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "token", token }, { "getnewtoken", getnewtoken.ToString() } });

                var response = await httpclient.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AbpResultModel>(responseString);
                accessTokenResult = result.result;
            }
            return accessTokenResult;
        }

        public static async Task<MessageResponseModel> GetMessageResponse(string token,int mpId,int messageId)
        {
            MessageResponseModel result = null;
            var url = JobConfig.AppSettings["GetMessageResultUrl"];
            var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
            using (HttpClient httpclient = new HttpClient(handler))
            {
                httpclient.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "token", token }, { "mpId", mpId.ToString() }, { "messageId", messageId.ToString() } });

                var response = await httpclient.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<MessageResponseModel>(responseString);
            }
            return result;
            
        }

    }
    public class AbpResultModel
    {
        public ApiTokenResult result { get; set; }
        public bool? success { get; set; }
        public bool? error { get; set; }
    }
}
