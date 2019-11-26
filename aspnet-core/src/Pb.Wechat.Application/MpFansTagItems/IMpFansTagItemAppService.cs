using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTagItems.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpFansTagItems
{
    public interface IMpFansTagItemAppService : IAsyncCrudAppService<MpFansTagItemDto, long, GetMpFansTagItemsInput, MpFansTagItemDto, MpFansTagItemDto>
    {
        Task<FileDto> GetListToExcel(GetMpFansTagItemsInput input);

        Task SaveFansTags(int mpId,int fansId,string tagIds);
    }
}
