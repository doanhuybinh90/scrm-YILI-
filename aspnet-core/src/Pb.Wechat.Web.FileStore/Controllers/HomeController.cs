using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Web.FileStore.Models;

namespace Pb.Wechat.Web.FileStore.Controllers
{
    public class HomeController : Controller
    {
        
      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> RemotingFileUpload(IFormFile file)
        {
            try
            {
                var type = Request.Query["type"].ToString();
                var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
                var relatepath = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/";
                var dirPath = $"{AppConfig.AppRoot}/{relatepath}";

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var webPath = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{AppConfig.FileDir}/{relatepath}";
                //var filename = $"{Guid.NewGuid().ToString()}.{extra}";
                byte[] fileBytes;
                using (var filestream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        filestream.CopyTo(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                }
                var fileInfo = new FileInfo(file.FileName);
                var tempFileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                var tempFilePath = Path.Combine(dirPath, tempFileName);
                await System.IO.File.WriteAllBytesAsync(tempFilePath, fileBytes);
                var url = $"{webPath}{tempFileName}";
                return Json(new { success = true, data = url });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
            //try
            //{
            //    var extra = Request.Query["extra"].ToString();
            //    var type = Request.Query["type"].ToString();
            //    var filename = $"{CreateTimeGuid()}.{extra}";
            //    var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
            //    var relatepath = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/";
            //    var dirPath = $"{AppConfig.AppRoot}/{relatepath}";
            //    var webPath = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{AppConfig.FileDir}/{relatepath}";
            //    Stream stream = HttpContext.Request.Body;
            //    byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
            //    stream.Read(buffer, 0, buffer.Length);
            //    string content = Encoding.UTF8.GetString(buffer);
            //    if (!Directory.Exists(dirPath))
            //        Directory.CreateDirectory(dirPath);
            //    var tempFileName = filename;
            //    var tempFilePath = Path.Combine(dirPath, tempFileName);
            //    using (FileStream fs = System.IO.File.Create(tempFilePath))
            //    {
            //        await fs.WriteAsync(buffer, 0, buffer.Length);
            //    }

            //    return Json(new { success = true, data = $"{webPath}{filename}" });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { success = false, data = ex.Message });
            //}
        }

        public async Task<JsonResult> RemotingFileUpload2()
        {
            try
            {
                var extra = Request.Query["extra"].ToString();
                var type = Request.Query["type"].ToString();
                var filename = $"{CreateTimeGuid()}.{extra}";
                var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
                var relatepath = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/";
                var dirPath = $"{AppConfig.AppRoot}/{relatepath}";
                var webPath = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{AppConfig.FileDir}/{relatepath}";
                Stream stream = HttpContext.Request.Body;
                byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                string content = Encoding.UTF8.GetString(buffer);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                var tempFileName = filename;
                var tempFilePath = Path.Combine(dirPath, tempFileName);
                using (FileStream fs = System.IO.File.Create(tempFilePath))
                {
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }

                return Json(new { success = true, data = $"{webPath}{filename}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }
        /// <summary>
        /// 创建一个按时间排序的Guid
        /// </summary>
        /// <returns></returns>
        public static string CreateTimeGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime now = DateTime.Now;

            DateTime baseDate = new DateTime(1900, 1, 1);

            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);

            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Copy(daysArray, 0, guidArray, 2, 2);
            //毫秒高位
            Array.Copy(msecsArray, 2, guidArray, 0, 2);
            //毫秒低位
            Array.Copy(msecsArray, 0, guidArray, 4, 2);
            return new System.Guid(guidArray).ToString();
        }

        public static DateTime GetDateTimeFromGuid(string strGuid)
        {
            Guid guid = Guid.Parse(strGuid);

            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays. 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        public async Task<ActionResult> DownloadFile()
        {
            FileStream file = new FileStream("E:\\Error.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(file);
            try
            {
                var fileName = Request.Query["fileName"].ToString();
                sw.WriteLine($"传入的filename={fileName}");
                string memoType = "";
                var extension = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "gif")
                    memoType = "image/jpeg";
                sw.WriteLine($"分析的cntentType={memoType}");
                uint state = 0;
                if (!Directory.Exists(AppConfig.mapDirPath))
                {
                    sw.WriteLine($"不存在物理地址{AppConfig.mapDirPath}");
               
                    try
                    {
                        sw.WriteLine($"用户名{AppConfig.mapDirUserName}");
                        sw.WriteLine($"密码{AppConfig.mapDirPwd}");
                        sw.WriteLine($"remoting地址{AppConfig.mapDirRemotingName}");
                        sw.WriteLine($"磁盘地址{AppConfig.mapDirPath}");
                        state = WNetHelper.WNetAddConnection(AppConfig.mapDirUserName, AppConfig.mapDirPwd, AppConfig.mapDirRemotingName, AppConfig.mapDirPath);
                    }
                    catch (Exception e)
                    {
                        sw.WriteLine($"链接虚拟磁盘报错：{e.Message}");
                        //Logger.Error($"网络地址错误：{e.Message}");
                        throw e;
                    }

                }
                if (state.Equals(0))
                {
                    sw.WriteLine($"打开存储地址{AppConfig.mapDirPath}");
                    byte[] bytes;
                    var fileFullName = Path.Combine(AppConfig.filePath, fileName);
                    sw.WriteLine($"文件全名{fileFullName}，开始存储文件");
                    using (var stream = System.IO.File.Open(fileFullName, FileMode.Open))
                    {
                        return File(stream, memoType);
                    }
                }
                else
                    sw.WriteLine($"未连接虚拟磁盘");
                return null;
            }
            catch (Exception ex)
            {
                sw.WriteLine($"整个过程报错：{ex.Message},堆栈：{ex.StackTrace}");
                throw ex;
            }
            finally
            {
                sw.Flush();
                file.Close();
                sw.Close();
            }
        }
    }
}
