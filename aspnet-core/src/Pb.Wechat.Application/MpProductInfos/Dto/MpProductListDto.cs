using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.MpProductInfos.Dto
{
    public class MpProductListDto
    {
        public string TypeTitle { get; set; }
        public string FilePathOrUrl { get; set; }
        public List<MpProductInfoDto> ProductList { get; set; }
    }
}
