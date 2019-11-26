using Abp;
using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFans.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpFans
{
    public interface IMpFanAppService : IAsyncCrudAppService<MpFanDto, int, GetMpFansInput, MpFanDto, MpFanDto>
    {
        Task<FileDto> GetListToExcel(GetMpFansInput input);
        Task<List<MpFanDto>> FindByNickName(string nickName);
        Task UpdateFansGroup(MpFanGroupDto input);
        Task<List<NameValue<string>>> GetCountrys();
        Task<List<NameValue<string>>> GetProvinces(CountryInputDto input);
        Task<List<NameValue<string>>> GetCitys(ProvinceInputDto input);
        Task<List<MpFanDto>> GetAllByMpId(int mpId);
        Task<List<MpFanDto>> GetAllByMpIdAndGroupId(int mpId, List<int> groupIds);
        Task<MpFanDto> GetFirstOrDefault(GetMpFansInput input);
        Task RenewFansGroup(MpFanGroupDto input);
        List<string> FilterDatas(int mpId,  List<string> openIds = null);
        Task<long> FilterCountAsync(int mpId);
        List<string> FilterNotMemberDatas(int mpId);
        List<string> FilterTagDatas(int mpId, string tagIds);
        Task<MpFanDto> GetFirstOrDefaultByOpenID(string openId);
    }
}
