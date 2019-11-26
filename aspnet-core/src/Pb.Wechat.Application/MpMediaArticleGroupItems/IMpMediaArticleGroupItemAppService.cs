using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticleGroupItems.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticleGroupItems
{
    public interface IMpMediaArticleGroupItemAppService : IAsyncCrudAppService<MpMediaArticleGroupItemDto,int,GetMpMediaArticleGrouItemInput, MpMediaArticleGroupItemDto, MpMediaArticleGroupItemDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaArticleGrouItemInput input);
        List<MpMediaArticleGroupItem> GetList();
        void Save(MpMediaArticleGroupItem input);
    }
}
