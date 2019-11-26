using Abp.AspNetCore.Mvc.Controllers;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pb.Wechat.Web.Models.ChunYuModel;
using Pb.Wechat.Web.Resources;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class ChunYuCallbackController : AbpController
    {
        private readonly IChunYuMessageHandler _chunYuMessageHandler;
        public ChunYuCallbackController(IChunYuMessageHandler chunYuMessageHandler)
        {
            _chunYuMessageHandler = chunYuMessageHandler;
        }

        [DontWrapResult]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> DoctorReply()
        {
            try
            {
                Logger.Info($"医生回复开始");
                Stream stream = Request.Body;
                byte[] buffer = new byte[Request.ContentLength ?? 0];
                stream.Read(buffer, 0, buffer.Length);
                string str = Encoding.UTF8.GetString(buffer);
                Logger.Info($"医生回复 {str}");
                var data = JsonConvert.DeserializeObject<DoctorReplyModel>(str);
                await _chunYuMessageHandler.Answer(data);
                return Json(new ChunYuReturnMsg()
                {
                    error = 0
                });
            }
            catch (Exception ex)
            {
                return Json(new ChunYuReturnMsg()
                {
                    error = 1,
                    error_msg = ex.Message
                });
            }
        }
        [DontWrapResult]
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> CloseProblem()
        {
            try
            {
                Logger.Info($"医生关闭开始");
                Stream stream = Request.Body;
                byte[] buffer = new byte[Request.ContentLength ?? 0];
                stream.Read(buffer, 0, buffer.Length);
                string str = Encoding.UTF8.GetString(buffer);
                Logger.Info($"医生关闭 {str}");
                var data = JsonConvert.DeserializeObject<CloseProblemModel>(str);
                await _chunYuMessageHandler.Close(data);
                return Json(new ChunYuReturnMsg()
                {
                    error = 0
                });
            }
            catch (Exception ex)
            {
                return Json(new ChunYuReturnMsg()
                {
                    error = 1,
                    error_msg = ex.Message
                });
            }
        }

        public IActionResult UserClose()
        {
            return View();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> DoClose(string openid)
        {
            return Json(new UserCloseModel() { State = (await _chunYuMessageHandler.Close(openid)) ? 1 : 0 });
        }
    }
}
