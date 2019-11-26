using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpGroupItems;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.YiliLastBuyProducts;
using Pb.Wechat.YiliMemberTypes;
using Pb.Wechat.YiliOfficialCitys;
using Pb.Wechat.YiliOrganizeCitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpGroups
{
    public interface IMpGroupAppService : IAsyncCrudAppService<MpGroupDto, int, GetMpGroupsInput, MpGroupDto, MpGroupDto>
    {
        Task<FileDto> GetListToExcel(GetMpGroupsInput input);
        Task<ListResultDto<MpGroupDto>> GetGroupUnits();
        Task<List<MpGroupDto>> GetList();
        Task Save(MpGroupDto input);
        Task<List<MpGroupDto>> GetListByIds(int mpid, List<int> ids);
        Task<ListResultDto<MpGroupDto>> GetMpGroupTrees();
        Task<MpGroupDto> MoveGroupParent(GetMpGroupsInput input);
        Task<List<NameValue<string>>> GetFirstLevelGroup(GetFirstLevelGroupInput input);
        Task<List<NameValue<string>>> GetSecondLevelGroup(GetSecondLevelGroupInput input);
        Task<List<MpGroupDto>> GetListByParentId(int mpId,int parentId);
        Task<MpGroupItemDto> GetGroupItemFirstOrDefault(GetGroupItemInput input);
        Task<MpGroupItemDto> GetItem(int parentId);
        Task<List<YiliOrganizeCity>> GetOrganizeCitys();
        Task<List<YiliOfficialCity>> GetOfficialCitys();
        Task<List<YiliLastBuyProduct>> GetLastBuyProducts();
        Task<List<YiliMemberType>> GetMemberTypes();
        Task SaveItem(MpGroupItemDto input);
    }
}
