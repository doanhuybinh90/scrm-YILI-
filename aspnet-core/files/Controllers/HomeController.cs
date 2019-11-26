using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace files.Controllers
{
    public class HomeController : Controller
    {
        private static string fileUrl = System.Configuration.ConfigurationManager.AppSettings["FileUrl"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Download()
        {
            var fileName = Request["fileName"].ToString();
            return Redirect($"{fileUrl}{fileName}");

            //FileStream file = new FileStream("E:\\Error.txt", FileMode.Open);
            //StreamWriter sw = new StreamWriter(file);
            //try
            //{
            //     fileName = Request["fileName"].ToString();
            //    //sw.WriteLine($"传入的filename={fileName}");
            //    string memoType = "";
            //    var extension = fileName.Substring(fileName.LastIndexOf(".") + 1);
            //    if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "gif")
            //        memoType = "image/jpeg";
            //    else if (extension == "mp4")
            //        memoType = "video/mpeg4";
            //    else if (extension == "rmvb")
            //        memoType = "application/vnd.rn-realmedia-vbr";
            //    else if (extension == "avi")
            //        memoType = "video/avi";
            //    else if (extension == "mpg")
            //        memoType = "video/mpg";
            //    else if (extension == "mp3")
            //        memoType = "audio/mp3";
            //    else if (extension == "wma")
            //        memoType = "audio/x-ms-wma";
            //    else if (extension == "wav")
            //        memoType = "application/x-shockwave-flash";
            //    else
            //        memoType = "text/plain";

            //    //sw.WriteLine($"分析的cntentType={memoType}");
            //    uint state = 0;
            //    if (!Directory.Exists(mapDirPath))
            //    {
            //        //Logger.Info("不存在该地址");
            //        try
            //        {
            //            state = WNetHelper.WNetAddConnection(mapDirUserName, mapDirPwd, mapDirRemotingName, mapDirPath);
            //        }
            //        catch (Exception e)
            //        {
            //            //Logger.Error($"网络地址错误：{e.Message}");
            //            throw e;
            //        }

            //    }
            //    if (state.Equals(0))
            //    {
                  
            //        byte[] bytes;
            //        var fileFullName = Path.Combine(filePath, fileName);
            //        //sw.WriteLine($"打开存储地址{fileFullName}");
            //        var stream = System.IO.File.Open(fileFullName, FileMode.Open);

            //        return File(stream, memoType);

            //    }

            //    return null;
            //}
            //catch (Exception ex)
            //{
               
            //    return Content($"错误：{ex.Message}");
               
            //    throw ex;
            //}
            //finally
            //{
            //    //sw.Flush();
            //    //file.Close();
            //    //sw.Close();
            //}
        }
    }
}