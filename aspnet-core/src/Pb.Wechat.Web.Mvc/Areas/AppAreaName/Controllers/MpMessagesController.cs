using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpMessages;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMessages;
using Pb.Wechat.MpGroups;
using Pb.Wechat.UserMps;
using Newtonsoft.Json;
using Pb.Wechat.MpEvents.Dto;
using Abp.BackgroundJobs;
using Pb.Hangfire.Jobs;
using Abp.Domain.Uow;
using Pb.Wechat.MpMessages.Dto;
using Abp.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpMessages)]
    public class MpMessagesController : AbpZeroTemplateControllerBase
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpMessageAppService _mpMessageAppService;
        private readonly IMpGroupAppService _mpGroupAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public MpMessagesController(IMpMessageAppService mpMessageAppService, IMpGroupAppService mpGroupAppService, IUserMpAppService userMpAppService, IBackgroundJobManager backgroundJobManager)
        {
            _mpMessageAppService = mpMessageAppService;
            _mpGroupAppService = mpGroupAppService;
            _userMpAppService = userMpAppService;
            _backgroundJobManager = backgroundJobManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _mpMessageAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMessageViewModel(output);
                viewModel.GroupModels = await _mpGroupAppService.GetList();
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID =await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpMessageViewModel();
                model.MpID = mpID;
                model.GroupModels = await _mpGroupAppService.GetList();
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public async Task<PartialViewResult> ContentSelector(int id)
        {
            var mpID =await _userMpAppService.GetDefaultMpId();
            ReplyPartialModel model = new ReplyPartialModel();
            if (id != 0)
            {
                var output = await _mpMessageAppService.Get(new EntityDto<int> { Id = id });
                model = JsonConvert.DeserializeObject<ReplyPartialModel>(JsonConvert.SerializeObject(output));
            }

            return PartialView("_ContentSelector", model);
        }

        public async Task<PartialViewResult> GetPartialDataListView(string type)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            if (type==MpMessageType.mpmultinews.ToString())
            {
                return PartialView("_MultiArticleDataTableList",new MpMessagePartialDataListModel {MessageType=type,MpID= mpid });
            }
            else
                return PartialView("_OtherDataTableList", new MpMessagePartialDataListModel { MessageType = type,MpID= mpid });
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task SendMessage(int id)
        {
            var data = await _mpMessageAppService.GetById(id);
            _backgroundJobManager.Enqueue<SendMessageImmediatelyJob, MpMessageDto>(data);
        }


        [DontWrapResult]
        public async Task<JsonResult> GetSecondLevelGroup(int mpId,int parentId)
        {
            var list = await _mpGroupAppService.GetSecondLevelGroup(new MpGroups.Dto.GetSecondLevelGroupInput { MpID= mpId, ParentID=parentId});
            var selectResult = list.Select(m => new { id = m.Value, text = m.Name }).ToList();
            return Json(selectResult);
        }
    }
}
