using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpMenus.Dto;
using Pb.Wechat.MpMenus.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMenus
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpMenuAppService : AsyncCrudAppService<MpMenu, MpMenuDto, int, GetMpMenusInput, MpMenuDto, MpMenuDto>, IMpMenuAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpMenuListExcelExporter _MpMenuListExcelExporter;
        private readonly IWxMediaAppService _wxMediaAppService;
        public MpMenuAppService(IRepository<MpMenu, int> repository, IMpMenuListExcelExporter MpMenuListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService) : base(repository)
        {
            _MpMenuListExcelExporter = MpMenuListExcelExporter;
            _userMpAppService = userMpAppService;

            _wxMediaAppService = wxMediaAppService;
        }

        protected override IQueryable<MpMenu> CreateFilteredQuery(GetMpMenusInput input)
        {
            var menutype = input.Type == null ? "" : input.Type.ToString();
            var msgtype = input.MediaType == null ? "" : input.MediaType.ToString();
            return Repository.GetAll()
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(input.Id != 0, c => c.Id == input.Id)
                .WhereIf(input.Type != null, c => c.Type == menutype)
                .WhereIf(input.MediaType != null, c => c.MediaType == msgtype)
                 .WhereIf(!string.IsNullOrWhiteSpace(input.MenuKey), c => c.MenuKey.Contains(input.MenuKey));
        }
        public async Task<FileDto> GetListToExcel(GetMpMenusInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpMenuListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpMenuDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();
            
            var entity =await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(id != 0, c => c.Id == id)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public async Task<ListResultDto<MpMenuDto>> GetMpMenuTrees()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var query =
                from ou in Repository.GetAll()
                where ou.IsDeleted == false && ou.MpID == mpid
                select new { ou };

            var items = await query.OrderBy(m=>m.ou.Id).ToListAsync();

            var result= new ListResultDto<MpMenuDto>(
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<MpMenuDto>(item.ou);
                    return dto;
                }).OrderBy(m=>m.Id).ToList());
            return result;
        }

        public async Task<MenuContentOutput> GetContentById(EntityDto<int> input)
        {
            var result = new MenuContentOutput();
            var data =await Repository.GetAsync(input.Id);
            if (data != null)
            {
                if (data.Type == MpMenuType.click.ToString())
                {
                    result.TypeName = "发送信息";
                }
                if (data.Type == MpMenuType.view.ToString())
                {
                    result.TypeName = "跳转到网页";
                    result.Content = data.LinkUrl;
                }

                if (data.MediaType == MpMessageType.image.ToString())
                {
                    result.MediaTypeName = "图片";
                    result.Content = data.ImageName;
                }
                if (data.MediaType == MpMessageType.mpmultinews.ToString())
                {
                    result.MediaTypeName = "多图文";
                    result.Content = data.ArticleGroupName;
                }
                if (data.MediaType == MpMessageType.mpnews.ToString())
                {
                    result.MediaTypeName = "图文";
                    result.Content = data.ArticleName;
                }
                if (data.MediaType == MpMessageType.none.ToString())
                {
                    result.MediaTypeName = "无";
                }
                if (data.MediaType == MpMessageType.text.ToString())
                {
                    result.MediaTypeName = "文本";
                    result.Content = data.Content;
                }
                if (data.MediaType == MpMessageType.video.ToString())
                {
                    result.MediaTypeName = "视频";
                    result.Content = data.VideoName;
                }
                if (data.MediaType == MpMessageType.voice.ToString())
                {
                    result.MediaTypeName = "音频";
                    result.Content = data.VoiceName;
                }

            }

            return result;
        }

        public Task<MpMenuDto> MoveMenuParent(GetMpMenusInput input)
        {
            var data = Repository.FirstOrDefault(m => m.Id == input.NewParentId);
            if (data != null)
                throw new System.Exception("对不起，只能支持两级目录的创建");
            var model = base.Get(new EntityDto<int> { Id = input.Id }).Result;
            model.ParentID = input.NewParentId;
            if (input.NewParentId != 0)
            {
                model.Length = 2;
                model.FullPath = data.FullPath + "." + model.Id;
                model.MenuFullPath = data.MenuFullPath + "." + model.Id;
            }
            else
            {
                model.Length = 1;
                model.FullPath =  model.Id.ToString();
                model.MenuFullPath =  model.Id.ToString();
            }
                

            return base.Update(model);
        }
        public override Task<MpMenuDto> Update(MpMenuDto input)
        {
            if (input.MenuKey == "service" || input.MenuKey == "doctorservice")
                input.MediaType = "";
            else
            {
                if (string.IsNullOrEmpty(input.MenuKey))
                    input.MenuKey = DateTime.Now.Ticks.ToString();
            }
            return base.Update(input);
        }
        public override Task<MpMenuDto> Create(MpMenuDto input)
        {
            var data = Repository.FirstOrDefault(m => m.Id == input.ParentID);
            if (data != null && data.ParentID != 0)
                throw new System.Exception("对不起，只能支持两级目录的创建");
            if (input.ParentID != 0)
                input.Length = 2;
            else
            {
                input.Length = 1;
            }

            if (string.IsNullOrWhiteSpace(input.MenuKey))
                input.MenuKey = DateTime.Now.Ticks.ToString();
            
            var result = base.Create(input).Result;
            if (input.ParentID != 0)
            {
                result.FullPath = input.ParentID + "." + result.Id;
                result.MenuFullPath = input.ParentID + "." + result.Id;
            }
            else
            {
                result.FullPath = result.Id.ToString();
                result.MenuFullPath = result.Id.ToString();
            }
            return Update(result);
        }

        public override Task Delete(EntityDto<int> input)
        {
            var _id = input.Id.ToString();
            var removeIds = Repository.GetAll().Where(m => m.IsDeleted == false && m.MenuFullPath.Contains(_id) && m.Id != input.Id).Select(m => m.Id).ToList();
            foreach (var item in removeIds)
            {
                Repository.Delete(item);
            }
            return base.Delete(input);
        }

        public async Task SyncMenu()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var list = Repository.GetAllList(m => m.IsDeleted == false && m.MpID == mpid).OrderBy(m=>m.Id).ToList();
            var result = await _wxMediaAppService.SyncMenu(list);
            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                Logger.Error(result.errcode.ToString() + result.errmsg);
                throw new System.Exception(result.errmsg);
            }

        }

        public async Task<MpMenuDto> GetEntityByInput(GetMpMenusInput input)
        {
            var query = CreateFilteredQuery(input);
            
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(query));
        }

        /// <summary>
        /// 新增菜单，移动菜单前判定已有按钮数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CheckInsertMenu(CheckMenuInput input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var count = Repository.GetAll().Where(m => m.MpID == mpid && m.IsDeleted == false && m.ParentID == input.PaerntId).Count();
            if (input.PaerntId == 0)
            {
                if (count > 2)
                    return false;
                else
                    return true;
            }
            else
            {
                if (count > 4)
                    return false;
                else
                    return true;
            }
        }

        public async Task<MpMenu> GetEntityByMenuKey(int mpid, string menuKey) {
            return await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(c => c.MpID == mpid && c.MenuKey == menuKey));
        }
    }
}
