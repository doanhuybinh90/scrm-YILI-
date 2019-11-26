using System.IO;
using System.Net;
using System.Text;

namespace Pb.Wechat.Web.Helpers
{
    public static class HttpLinkHelper
    {
        #region Post通用方法
        //Post数据
        public static string PostToURL_UTF8(string PostURL, string Data)
        {
            string retString = "";
            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostURL);
            byte[] postData = Encoding.GetEncoding("utf-8").GetBytes(Data);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = postData.Length;
            using (System.IO.Stream sw = request.GetRequestStream())
            {
                sw.Write(postData, 0, postData.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                retString = sr.ReadToEnd();
            }
            return retString;
        }

        //Post数据
        public static string PostToURL_GBK(string PostURL, string Data)
        {
            string retString = "";
            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostURL);
            byte[] postData = Encoding.GetEncoding("GBK").GetBytes(Data);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=GBK";
            request.ContentLength = postData.Length;
            using (System.IO.Stream sw = request.GetRequestStream())
            {
                sw.Write(postData, 0, postData.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
            {
                retString = sr.ReadToEnd();
            }
            return retString;
        }
        #endregion

        #region Get通用方式
        /// <summary>
        /// Get方式
        /// </summary>
        /// <param name="Url">地址</param>
        /// <param name="postDatas">发送数据</param>
        /// <returns></returns>
        public static string GetToURL_Utf8(string Url, string postDatas)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url + (postDatas == "" ? "" : "?") + postDatas);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string responseString = reader.ReadToEnd();
            reader.Close();
            myResponseStream.Close();
            return responseString;
        }

        #endregion
    }
}
