using Abp.IO.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// 微信素材Post请求
        /// </summary>
        /// <param name="url">接口url地址</param>
        /// <param name="bytes">文件素材字节</param>
        /// <param name="filename">文件名</param>
        /// <param name="timeOut">超时时长</param>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, byte[] bytes, string filename, int timeOut = -1)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (timeOut > 0)
                request.Timeout = timeOut;

            #region 处理Form表单文件上传

            //通过表单上传文件
            var postStream = new MemoryStream();

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            string dataFormdataTemplate = "\r\n--" + boundary +
                                          "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            try
            {
                string formdata = null;
                formdata = string.Format(fileFormdataTemplate, "media", filename);

                //统一处理
                var formdataBytes = Encoding.UTF8.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                await postStream.WriteAsync(formdataBytes, 0, formdataBytes.Length);
                await postStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //结尾
            var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            await postStream.WriteAsync(footer, 0, footer.Length);

            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            #endregion

            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    await requestStream.WriteAsync(buffer, 0, bytesRead);
                }


                //debug
                postStream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(postStream);
                var postStr = await sr.ReadToEndAsync();


                postStream.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string retString = await myStreamReader.ReadToEndAsync();
                    return retString;
                }
            }
        }

        /// <summary>
        /// 微信素材Post请求
        /// </summary>
        /// <param name="url">接口url地址</param>
        /// <param name="stream">文件素材流</param>
        /// <param name="filename">文件名</param>
        /// <param name="timeOut">超时时长</param>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, Stream stream, string filename, int timeOut = -1)
        {
            return await HttpPost(url, stream.GetAllBytes(), filename, timeOut);
        }

        /// <summary>
        /// 微信素材Post请求
        /// </summary>
        /// <param name="url">接口url地址</param>
        /// <param name="fileurl">文件素材url地址</param>
        /// <param name="filename">文件名</param>
        /// <param name="timeOut">超时时长</param>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, string fileurl, string filename = "", int timeOut = -1)
        {
            return await HttpPost(url, await new WebClient().DownloadDataTaskAsync(fileurl), string.IsNullOrEmpty(filename) ? fileurl.Split('/').LastOrDefault() : filename, timeOut);
        }

        public static async Task<string> HttpPostVideo(string url, byte[] bytes, string filename, KeyValuePair<string, string> fileDictionary, int timeOut = -1)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (timeOut > 0)
                request.Timeout = timeOut;

            #region 处理Form表单文件上传

            //通过表单上传文件
            var postStream = new MemoryStream();

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            string dataFormdataTemplate = "\r\n--" + boundary +
                                          "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            try
            {
                string formdata = null;
                formdata = string.Format(fileFormdataTemplate, "media", filename);

                //统一处理
                var formdataBytes = Encoding.UTF8.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                await postStream.WriteAsync(formdataBytes, 0, formdataBytes.Length);
                await postStream.WriteAsync(bytes, 0, bytes.Length);



                string formdataX = null;
                formdataX = string.Format(dataFormdataTemplate, fileDictionary.Key, fileDictionary.Value);

                //统一处理
                var formdataBytesX = Encoding.UTF8.GetBytes(postStream.Length == 0 ? formdataX.Substring(2, formdataX.Length - 2) : formdataX);//第一行不需要换行
                await postStream.WriteAsync(formdataBytesX, 0, formdataBytesX.Length);
               


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //结尾
            var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            await postStream.WriteAsync(footer, 0, footer.Length);

            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            #endregion

            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    await requestStream.WriteAsync(buffer, 0, bytesRead);
                }


                //debug
                postStream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(postStream);
                var postStr = await sr.ReadToEndAsync();


                postStream.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string retString = await myStreamReader.ReadToEndAsync();
                    return retString;
                }
            }
        }

        public static async Task<string> HttpPostVideo(string url, Stream stream, string filename, KeyValuePair<string, string> fileDictionary, int timeOut = -1)
        {
            return await HttpPostVideo(url, stream.GetAllBytes(), filename, fileDictionary, timeOut);
        }
        public static async Task<string> HttpPostVideo(string url, string fileurl, KeyValuePair<string, string> fileDictionary, string filename = "", int timeOut = -1)
        {
            return await HttpPostVideo(url, await new WebClient().DownloadDataTaskAsync(fileurl), string.IsNullOrEmpty(filename) ? fileurl.Split('/').LastOrDefault() : filename, fileDictionary, timeOut);
        }
    }
}
