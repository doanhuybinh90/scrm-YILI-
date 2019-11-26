using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、服务和配置文件中的类名“Service”。
public class Service : IService
{
    //public string GetData(int value)
    //{
    //	return string.Format("You entered: {0}", value);
    //}

    //public CompositeType GetDataUsingDataContract(CompositeType composite)
    //{
    //	if (composite == null)
    //	{
    //		throw new ArgumentNullException("composite");
    //	}
    //	if (composite.BoolValue)
    //	{
    //		composite.StringValue += "Suffix";
    //	}
    //	return composite;
    //}
    private static string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ScrmConnectionString"];
    private static string QrCodeUrl = System.Configuration.ConfigurationManager.AppSettings["QrCodeUrl"];
    private static string Token= System.Configuration.ConfigurationManager.AppSettings["token"];
    /// <summary>
    /// 生成临时二维码(有大效期30天)(微信)
    /// </summary>
    /// <param name="OpenId">公众号ID</param>
    /// <param name="scene_id">参数值（妈妈班活动ID+110000）</param>
    /// <param name="expire_seconds">二维码有效期</param>
    /// <returns>二维码对应Ticket值</returns>
    public async Task<string> GetQRCodeTicket(string OpenId, int scene_id, int expire_seconds)
    {
        try
        {
            var eventKey = (scene_id + 110000).ToString();
            var result = GetQrCode(Token, eventKey, 0, expire_seconds);
            return result.ticket;
        }
        catch (Exception ex)
        {

            return ex.Message;
        }
        

    }
    /// <summary>
    /// 根据门店ID或导购ID生成永久二维码
    /// </summary>
    /// <param name="OpenId">公众号ID</param>
    /// <param name="">申请来源（1：会员中心；2：商家中心）</param>
    /// <param name="">分配类型（1:门店；2：导购；3：代表）</param>
    /// <param name="">分配关联ID(门店或导购ID)</param>
    /// <returns>二维码对应Ticket值</returns>
    public async Task<string> GetQRCodeLimitTicketEx(string OpenId, int ApplySource, int AssignType, int AssignRelateID)
    {
        try
        {
            var eventKey = AssignRelateID .ToString();
            var result = GetQrCode(Token, eventKey, 1, 0);
            return result.ticket;
            
        }
        catch (Exception ex)
        {

            return ex.Message;
        }
    }

    /// <summary>
    /// 获取带参二维码
    /// </summary>
    /// <returns></returns>
    private static CreateQrCodeResult GetQrCode(string token, string eventKey,int isLimit,int expire_seconds)
    {
        CreateQrCodeResult qcCodeResult = null;
        var url = QrCodeUrl;
        var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
        using (HttpClient httpclient = new HttpClient(handler))
        {
            httpclient.BaseAddress = new Uri(url);
            var content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "token", token }, { "eventKey", eventKey },{ "isLimit",isLimit.ToString() },{ "expire_seconds", expire_seconds.ToString() } });

            var response = httpclient.PostAsync(url, content);

            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            qcCodeResult = JsonConvert.DeserializeObject<CreateQrCodeResult>(responseString);
        }
        return qcCodeResult;
    }
}
