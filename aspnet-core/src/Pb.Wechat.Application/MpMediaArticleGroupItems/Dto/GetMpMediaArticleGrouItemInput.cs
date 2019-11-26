using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticleGroupItems.Dto
{
    public class GetMpMediaArticleGrouItemInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int GroupID { get; set; }
        public string Name { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id ASC";
            }
        }
    }
}
