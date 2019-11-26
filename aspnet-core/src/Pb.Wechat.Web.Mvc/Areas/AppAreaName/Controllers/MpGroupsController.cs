using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpGroups;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpGroups;
using Pb.Wechat.UserMps;
using Microsoft.AspNetCore.Hosting;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Linq;
using Abp.Web.Models;
using System;
using Pb.Wechat.MpChannels;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpFansTags;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpUserManagement_MpGroups)]
    public class MpGroupsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpGroupAppService _mpGroupAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IHostingEnvironment _host = null;
        private readonly IMpChannelAppService _mpChannelAppService;
        private readonly IMpFansTagAppService _mpFansTagAppService;
        public MpGroupsController(IMpGroupAppService mpGroupAppService, IUserMpAppService userMpAppService, IHostingEnvironment host, IMpChannelAppService mpChannelAppService, IMpFansTagAppService mpFansTagAppService)
        {
            _mpGroupAppService = mpGroupAppService;
            _userMpAppService = userMpAppService;
            _host = host;
            _mpChannelAppService = mpChannelAppService;
            _mpFansTagAppService = mpFansTagAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpGroupViewModel();
            var list = (await _mpGroupAppService.GetList()).OrderBy(m => m.FullPath).ToList();
            viewModel.Groups = list;
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id, int parentId, string parentName)
        {
            if (id != 0)
            {
                var output = await _mpGroupAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpGroupViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID = await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpGroupViewModel() { MpID = mpID, ParentID = parentId, ParentName = parentName };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public PartialViewResult SelectorModal()
        {
            return PartialView("_SelectorModal");
        }

        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> GetGroupSelector()
        {
            var dataList = await _mpGroupAppService.GetList();

            return Json(GetTree(dataList, 0));
        }

        private List<GroupSelectModal> GetTree(List<MpGroupDto> list, int parentId)
        {
            List<GroupSelectModal> trees = new List<GroupSelectModal>();
            var dataList = list.Where(m => m.ParentID == parentId);
            foreach (var data in dataList)
            {
                GroupSelectModal model = new GroupSelectModal();
                model.id = data.Id;
                model.name = data.Name;
                if (list.Any(m => m.ParentID == data.Id))
                    model.children = GetTree(list, model.id);
                trees.Add(model);
            }
            return trees;
        }

        [IgnoreAntiforgeryToken]
        public async Task<PartialViewResult> TreeData(string name)
        {
            var list = (await _mpGroupAppService.GetList()).OrderBy(m => m.FullPath).ToList();
            if (!string.IsNullOrEmpty(name))
                list = list.Where(m => m.Name.Contains(name)).ToList();
            return PartialView("_TreeData", list);
        }

        public async Task<PartialViewResult> CreateItem(int id, string parentName)
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            var output = await _mpGroupAppService.GetItem(id);
            if (output==null)
            {
                output = new MpGroups.Dto.MpGroupItemDto();
                output.MpID = mpID;
                output.Name = parentName;
                output.ParentID = id;
                output.BeginPointsBalance = -1;
                output.EndPointsBalance = -1;
                output.BeginBabyBirthday = new DateTime(2000,1,1);
                output.EndBabyBirthday = new DateTime(2000, 1, 2);
            }
            
            var viewModel = new CreateOrEditMpGroupItemViewModel(output);
            return PartialView("_CreateOrEditItemModal", viewModel);

        }


        [DontWrapResult]
        public async Task<JsonResult> GetOrganizeCitys()
        {
            var list = await _mpGroupAppService.GetOrganizeCitys();
            var selectResult = list.Select(m => new { id = m.Name, text = m.Name }).ToList();
            return Json(selectResult);
        }
        [DontWrapResult]
        public async Task<JsonResult> GetOfficialCitys()
        {
            var list = await _mpGroupAppService.GetOfficialCitys();
            var selectResult = list.Select(m => new { id = m.Name, text = m.Name }).ToList();
            return Json(selectResult);
        }
        [DontWrapResult]
        public async Task<JsonResult> GetLastBuyProducts()
        {
            var list = await _mpGroupAppService.GetLastBuyProducts();
            var selectResult = list.Select(m => new { id = m.Code, text = m.Name });
            return Json(selectResult);
        }

        [DontWrapResult]
        public async Task<JsonResult> GetChannels()
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            var list = await _mpChannelAppService.GetAllChannel(new MpChannels.Dto.GetMpChannelsInput { MpID= mpID });
            var selectResult = list.Select(m => new { id = m.Id, text = m.Name });
            return Json(selectResult);
            
        }
        [DontWrapResult]
        public async Task<JsonResult> GetMemberTypes()
        {
            var list = await _mpGroupAppService.GetMemberTypes();
            var selectResult = list.Select(m => new { id = m.Name, text = m.Name });
            return Json(selectResult);
        }

        [DontWrapResult]
        public async Task<JsonResult> GetFansTags()
        {
            var list = await _mpFansTagAppService.GetAllTags(new MpFansTags.Dto.GetMpFansTagsInput());
            var selectResult = list.Select(m => new { id = m.Id, text = m.Name }).ToList();
            return Json(selectResult);
        }
    }
}
