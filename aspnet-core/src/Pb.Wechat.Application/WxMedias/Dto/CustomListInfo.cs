using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.WxMedias.Dto
{
    public class CustomListInfo
    {
        /// <summary>
		/// 客服列表
		/// </summary>
		public List<CustomInfo_Json> kf_list { get; set; }
    }
    public class CustomInfo_Json
    {
        /// <summary>
		/// 客服账号
		/// </summary>
		public string kf_account { get; set; }

        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick { get; set; }

        /// <summary>
        /// 客服工号
        /// </summary>
        public int kf_id { get; set; }

        /// <summary>
        /// 客服头像
        /// </summary>
        public string kf_headimgurl { get; set; }
        /// <summary>
        /// 客服微信
        /// </summary>
        public string kf_wx { get; set; }

    }
}
