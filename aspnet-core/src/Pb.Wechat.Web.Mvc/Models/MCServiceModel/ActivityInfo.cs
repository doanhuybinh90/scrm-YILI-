namespace Pb.Wechat.Web.Models.MCServiceModel
{
    public class ActivityInfo
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public int ID = 0;

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityCode = "";
        /// <summary>
        /// 活动名称
        /// </summary>
        public string Topic = "";

        /// <summary>
        /// 活动简介
        /// </summary>
        public string ActivityIntroduce = "";

        /// <summary>
        /// 活动类型
        /// </summary>
        public int Classify = 0;


        /// <summary>
        /// 活动举办行政城市
        /// </summary>
        public int OfficialCity = 0;

        /// <summary>
        /// 默认门店
        /// </summary>
        public int DefaultClient = 0;
        public string DefaultClientCode = "";
        public string DefaultClientFullName = "";
    }
}
