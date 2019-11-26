using System;

namespace Pb.Wechat.Web.Models.MCServiceModel
{
    /// <summary>
    /// Member:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Member
    {
        public Member()
        { }
        #region Model
        private int _id;
        private int _fansid = 0;
        private string _username = "";
        private string _password = "";
        private int _usertype = 0;
        private DateTime? _babybirthday;
        private string _phone = "";
        private string _email = "";
        private int _crmid = 0;
        private int _sourceid = 0;
        private DateTime _createtime = DateTime.Now;
        private DateTime? _updatetime;
        private int _updateid = 0;
        private int _productid = 0;
        private string _realname = "";
        private bool _istest = false;
        private int _state;
        private string _unbindreason;
        private bool _needRebind = false;
        /// <summary>
        /// 自动编号
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 粉丝编号
        /// </summary>
        public int FansId
        {
            set { _fansid = value; }
            get { return _fansid; }
        }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType
        {
            set { _usertype = value; }
            get { return _usertype; }
        }
        /// <summary>
        /// 宝宝生日或预产期
        /// </summary>
        public DateTime? BabyBirthday
        {
            set { _babybirthday = value; }
            get { return _babybirthday; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// CRM系统编号
        /// </summary>
        public int CRMId
        {
            set { _crmid = value; }
            get { return _crmid; }
        }
        /// <summary>
        /// CRM系统来源编号
        /// </summary>
        public int SourceId
        {
            set { _sourceid = value; }
            get { return _sourceid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 更新者
        /// </summary>
        public int UpdateId
        {
            set { _updateid = value; }
            get { return _updateid; }
        }
        /// <summary>
        /// 注册产品
        /// </summary>
        public int ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 是否是测试数据
        /// </summary>
        public bool IsTest
        {
            set { _istest = value; }
            get { return _istest; }
        }

        /// <summary>
        /// State
        /// </summary>		

        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// UnBindReason
        /// </summary>        
        public string UnBindReason
        {
            get { return _unbindreason; }
            set { _unbindreason = value; }
        }

        /// <summary>
        /// 是否需要重新绑定
        /// </summary>
        public bool NeedRebind
        {
            get { return _needRebind; }
            set { _needRebind = value; }
        }
        #endregion Model

    }
}
