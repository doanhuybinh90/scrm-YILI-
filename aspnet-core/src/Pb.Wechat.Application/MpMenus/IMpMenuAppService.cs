using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMenus.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMenus
{
    public interface IMpMenuAppService : IAsyncCrudAppService<MpMenuDto, int, GetMpMenusInput, MpMenuDto, MpMenuDto>
    {
        Task<FileDto> GetListToExcel(GetMpMenusInput input);
        Task<MpMenuDto> GetModelByReplyTypeAsync(int id, int mpId);
        Task<ListResultDto<MpMenuDto>> GetMpMenuTrees();
        Task<MenuContentOutput> GetContentById(EntityDto<int> input);

        Task<MpMenuDto> MoveMenuParent(GetMpMenusInput input);
        Task SyncMenu();
        Task<MpMenuDto> GetEntityByInput(GetMpMenusInput input);

        Task<bool> CheckInsertMenu(CheckMenuInput input);
        Task<MpMenu> GetEntityByMenuKey(int mpid, string menuKey);
    }
}
