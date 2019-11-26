namespace Pb.Wechat.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents= "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        public const string Pages_MpManagers = "Pages.MpManagers";
        public const string Pages_MpManagers_MpAccounts = "Pages.MpManagers.MpAccounts";
        public const string Pages_MpManagers_MpApiTokens = "Pages.MpManagers.MpApiTokens";
        public const string Pages_MpManagers_MpChannels = "Pages.MpManagers.MpChannels";

        public const string Pages_ReplyManagement = "Pages.ReplyManagement";
        public const string Pages_ReplyManagement_MpEvents = "Pages.ReplyManagement.MpEvents";
        public const string Pages_ReplyManagement_MpSelfArticles = "Pages.ReplyManagement.MpSelfArticles";
        public const string Pages_ReplyManagement_MpSelfArticleGroups = "Pages.ReplyManagement.MpSelfArticleGroups";
        public const string Pages_ReplyManagement_MpSelfArticleGroupItems = "Pages.ReplyManagement.MpSelfArticleGroupItems";
        public const string Pages_ReplyManagement_MpKeyWordReplys = "Pages.ReplyManagement.MpKeyWordReplys";
        public const string Pages_ReplyManagement_MpFocus = "Pages.ReplyManagement.MpFoucus";
        

        public const string Pages_MaterialManage = "Pages.MaterialManage";
        public const string Pages_MaterialManage_MpMediaVideos = "Pages.MaterialManage.MpMediaVideos";
        public const string Pages_MaterialManage_MpMediaImages = "Pages.MaterialManage.MpMediaImages";
        public const string Pages_MaterialManage_MpMediaVoices = "Pages.MaterialManage.MpMediaVoices";
        public const string Pages_MaterialManage_MpMediaArticles = "Pages.MaterialManage.MpMediaArticles";
        public const string Pages_MaterialManage_MpMediaArticleGroups = "Pages.MaterialManage.MpMediaArticleGroups";
        public const string Pages_MaterialManage_MpMediaArticleGroupItems = "Pages.MaterialManage.MpMediaArticleGroupItems";
        public const string Pages_MaterialManage_MpMediaImageTypes = "Pages.MaterialManage.MpMediaImageTypes";

        public const string Pages_CustomerService = "Pages.CustomerService";
        public const string Pages_CustomerService_CustomerServiceOnline = "Pages.CustomerService.CustomerServiceOnline";
        public const string Pages_CustomerService_CustomerServiceResponseText = "Pages.CustomerService.CustomerServiceResponseText";
        public const string Pages_CustomerService_CustomerServiceWorkTime = "Pages.CustomerService.CustomerServiceWorkTime";
        public const string Pages_CustomerService_CustomerServiceResponseTypes = "Pages.CustomerService.CustomerServiceResponseTexts";
        public const string Pages_CustomerService_CustomerServiceReports = "Pages.CustomerService.CustomerServiceReports";
        public const string Pages_CustomerService_CustomerServiceConversations = "Pages.CustomerService.CustomerServiceConversations";

        public const string Pages_CustomerService_CustomerMartial = "Pages.CustomerService.CustomerMartial";
        public const string Pages_CustomerService_CustomerMartial_CustomerMediaImages = "Pages.CustomerService.CustomerMartial.CustomerMediaImages";
        public const string Pages_CustomerService_CustomerMartial_CustomerMediaVideos = "Pages.CustomerService.CustomerMartial.CustomerMediaVideos";
        public const string Pages_CustomerService_CustomerMartial_CustomerMediaVoices = "Pages.CustomerService.CustomerMartial.CustomerMediaVoices";
        public const string Pages_CustomerService_CustomerMartial_CustomerArticles = "Pages.CustomerService.CustomerMartial.CustomerArticles";
        public const string Pages_CustomerService_CustomerMartial_CustomerArticleGroups = "Pages.CustomerService.CustomerMartial.CustomerArticleGroups";

        public const string Pages_MpUserManagement = "Pages.MpUserManagement";
        public const string Pages_MpUserManagement_MpFansTags = "Pages.MpUserManagement.MpFansTags";
        public const string Pages_MpUserManagement_MpFans = "Pages.MpUserManagement.MpFans";
        public const string Pages_MpUserManagement_MpGroups = "Pages.MpUserManagement.MpGroups";
        public const string Pages_MpUserManagement_MpUserMembers = "Pages.MpUserManagement.MpUserMembers";

        public const string Pages_MpUserMembers = "Pages.MpUserMembers";
        
        public const string Pages_MpMenus = "Pages.MpMenus";
        public const string Pages_MpMessages = "Pages.MpMessages";

        public const string Pages_MpBabyText = "Pages.MpBabyText";
        public const string Pages_MpSolicitude = "Pages.MpSolicitude";
        public const string Pages_MpSolicitude_Settings = "Pages.MpSolicitude.Settings";
        public const string Pages_MpSolicitude_Texts = "Pages.MpSolicitude.Texts";
        public const string Pages_MpShoppingMallPic = "Pages.MpShoppingMallPic";
        public const string Pages_MpProducts = "Pages.MpProducts";
        public const string Pages_MpProducts_MpProductInfos = "Pages.MpProducts.MpProductInfos";
        public const string Pages_MpProducts_MpProductTypes = "Pages.MpProducts.MpProductTypes";

        public const string Pages_CY = "Pages.CY";
        public const string Pages_CY_Configs = "Pages.CY.Configs";
        public const string Pages_CY_Problems = "Pages.CY.Problems";
        public const string Pages_CY_ProblemContents = "Pages.CY.ProblemContents";
        public const string Pages_CY_Doctors = "Pages.CY.Doctors";
        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

    }
}