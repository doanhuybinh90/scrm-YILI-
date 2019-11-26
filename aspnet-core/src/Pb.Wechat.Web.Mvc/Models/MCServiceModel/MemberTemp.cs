namespace Pb.Wechat.Web.Models.MCServiceModel
{
    public class MemberTemp
    {
        private System.Guid idField;

        private string realNameField;

        private int memberTypeField;

        private string mobileField;

        private string emailField;

        private bool emailVerifyFlagField;

        private int sexField;

        private string birthdayField;

        private int homeRoleField;

        private string babyNameField;

        private string hobbyField;

        private string mamaGroup;

        private string serviceCMClientCode;

        private int infocollectactivityid;

        private int officialCityField;


        private string addressField;

        private string activeDateField;

        private int registerSourceField;


        private int cRMIDField;

        private Attachment[] attsField;

        private string collectProductField;

        private string serviceCMClientName;

        private string servicePromotorName;

        private string servicePromotorMobile;

        private int preBrand;

        private int activityInfoSource;


        public System.Guid ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }


        public string RealName
        {
            get
            {
                return this.realNameField;
            }
            set
            {
                this.realNameField = value;
            }
        }


        public int MemberType
        {
            get
            {
                return this.memberTypeField;
            }
            set
            {
                this.memberTypeField = value;
            }
        }


        public string Mobile
        {
            get
            {
                return this.mobileField;
            }
            set
            {
                this.mobileField = value;
            }
        }


        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        public bool EmailVerifyFlag
        {
            get
            {
                return this.emailVerifyFlagField;
            }
            set
            {
                this.emailVerifyFlagField = value;
            }
        }

        public int Sex
        {
            get
            {
                return this.sexField;
            }
            set
            {
                this.sexField = value;
            }
        }

        public string Birthday
        {
            get
            {
                return this.birthdayField;
            }
            set
            {
                this.birthdayField = value;
            }
        }

        public int HomeRole
        {
            get
            {
                return this.homeRoleField;
            }
            set
            {
                this.homeRoleField = value;
            }
        }

        public string BabyName
        {
            get
            {
                return this.babyNameField;
            }
            set
            {
                this.babyNameField = value;
            }
        }

        public string Hobby
        {
            get
            {
                return this.hobbyField;
            }
            set
            {
                this.hobbyField = value;
            }
        }

        public string MaMaGroup
        {
            get { return this.mamaGroup; }
            set { this.mamaGroup = value; }
        }

        public string ServiceCMClientCode
        {
            get
            {
                return this.serviceCMClientCode;
            }
            set
            {
                this.serviceCMClientCode = value;
            }
        }


        public string ServiceCMClientName
        {
            get
            {
                return this.serviceCMClientName;
            }
            set
            {
                this.serviceCMClientName = value;
            }
        }


        public string ServicePromotorName
        {
            get
            {
                return this.servicePromotorName;
            }
            set
            {
                this.servicePromotorName = value;
            }
        }


        public string ServicePromotorMobile
        {
            get
            {
                return this.servicePromotorMobile;
            }
            set
            {
                this.servicePromotorMobile = value;
            }
        }


        public int InfoCollectActivity
        {
            get { return this.infocollectactivityid; }
            set { this.infocollectactivityid = value; }
        }

        public int OfficialCity
        {
            get
            {
                return this.officialCityField;
            }
            set
            {
                this.officialCityField = value;
            }
        }

        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }


        public string ActiveDate
        {
            get
            {
                return this.activeDateField;
            }
            set
            {
                this.activeDateField = value;
            }
        }

        /// <summary>
        /// 注册来源
        /// </summary>

        public int RegisterSource
        {
            get
            {
                return this.registerSourceField;
            }
            set
            {
                this.registerSourceField = value;
            }
        }

        /// <summary>
        /// 注册产品
        /// </summary>

        public string CollectProduct
        {
            get
            {
                return this.collectProductField;
            }
            set
            {
                this.collectProductField = value;
            }
        }


        public int CRMID
        {
            get
            {
                return this.cRMIDField;
            }
            set
            {
                this.cRMIDField = value;
            }
        }

        /// <summary>
        /// 之前使用品牌
        /// </summary>

        public int PreBrand
        {
            get { return this.preBrand; }
            set { this.preBrand = value; }
        }

        /// <summary>
        /// 活动信息来源（嘉年华）
        /// </summary>

        public int ActivityInfoSource
        {
            get { return this.activityInfoSource; }
            set { this.activityInfoSource = value; }
        }


        public Attachment[] Atts
        {
            get
            {
                return this.attsField;
            }
            set
            {
                this.attsField = value;
            }
        }
    }
}
