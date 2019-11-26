using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpAccounts.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpAccounts
{
    public interface IMpAccountAppService : IAsyncCrudAppService<MpAccountDto, int, GetMpAccountsInput, MpAccountDto, MpAccountDto>
    {
        Task<FileDto> GetListToExcel(GetMpAccountsInput input);
        Task<MpAccountDto> GetFirstOrDefault();
        Task<List<MpAccountDto>> GetList();
        Task<MpAccountDto> GetCache(int id);


    }
}
