using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpUserMembers.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpUserMembers
{
    public interface IMpUserMemberAppService : IAsyncCrudAppService<MpUserMemberDto, int, GetMpUserMembersInput, MpUserMemberDto, MpUserMemberDto>
    {
        Task<FileDto> GetListToExcel(GetMpUserMembersInput input);
        Task<MpUserMemberDto> GetByOpenID(string openId,bool renew=false);
        Task<string> GetMgccAuthKey(string openId);
        Task<List<string>> FilterOpenIds(string motherType,List<string> openIds);
        Task DeleteOtherSame(string openId, int id);
        Task<List<MpUserMemberDto>> GetList(GetMpUserMembersInput input);
        Task<MpUserMemberDto> UnBinding(UnBindindInput input);
        Task<MpUserMemberDto> GetByMobile(string mobile);
    }
}
