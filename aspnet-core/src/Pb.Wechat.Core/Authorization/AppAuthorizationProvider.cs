using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Pb.Wechat.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            //pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));
           
           


            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));

            //TENANT-SPECIFIC PERMISSIONS

            //pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);


            var mpmanages = pages.CreateChildPermission(AppPermissions.Pages_MpManagers, L("MpManagers"));
            mpmanages.CreateChildPermission(AppPermissions.Pages_MpManagers_MpAccounts, L("MpAccounts"));
            mpmanages.CreateChildPermission(AppPermissions.Pages_MpManagers_MpApiTokens, L("MpApiTokens"));
            mpmanages.CreateChildPermission(AppPermissions.Pages_MpManagers_MpChannels, L("MpChannels"));

            var replyManagement = pages.CreateChildPermission(AppPermissions.Pages_ReplyManagement, L("ReplyManagement"));
            replyManagement.CreateChildPermission(AppPermissions.Pages_ReplyManagement_MpSelfArticles, L("MpSelfArticles"));
            replyManagement.CreateChildPermission(AppPermissions.Pages_ReplyManagement_MpSelfArticleGroups, L("MpSelfArticleGroups"));
            //replyManagement.CreateChildPermission(AppPermissions.Pages_ReplyManagement_MpSelfArticleGroupItems, L("MpSelfArticleGroupItems"));
            replyManagement.CreateChildPermission(AppPermissions.Pages_ReplyManagement_MpEvents, L("MpEvents"));
            replyManagement.CreateChildPermission(AppPermissions.Pages_ReplyManagement_MpFocus, L("MpFoucus"));
            replyManagement.CreateChildPermission(AppPermissions.Pages_ReplyManagement_MpKeyWordReplys, L("MpKeyWordReplys"));

            var materialManage = pages.CreateChildPermission(AppPermissions.Pages_MaterialManage, L("MaterialManage"));
            materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaVideos, L("MpMediaVideos"));
            materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaImages, L("MpMediaImages"));
            materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaImageTypes, L("MpMediaImageTypes"));
            materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaVoices, L("MpMediaVoices"));
            materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaArticles, L("MpMediaArticles"));
            materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaArticleGroups, L("MpMediaArticleGroups"));
            //materialManage.CreateChildPermission(AppPermissions.Pages_MaterialManage_MpMediaArticleGroupItems, L("MpMediaArticleGroupItems"));

            var userManagement = pages.CreateChildPermission(AppPermissions.Pages_MpUserManagement, L("MpUserManagement"));
            userManagement.CreateChildPermission(AppPermissions.Pages_MpUserManagement_MpFansTags, L("MpFansTags"));
            userManagement.CreateChildPermission(AppPermissions.Pages_MpUserManagement_MpFans, L("MpFans"));
            userManagement.CreateChildPermission(AppPermissions.Pages_MpUserManagement_MpUserMembers, L("MpUserMembers"));
            userManagement.CreateChildPermission(AppPermissions.Pages_MpUserManagement_MpGroups, L("MpGroups"));

            var customerService=pages.CreateChildPermission(AppPermissions.Pages_CustomerService, L("CustomerService"));
            customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerServiceOnline, L("CustomerServiceOnline"));
            customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerServiceResponseText, L("CustomerServiceResponseText"));
            customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerServiceResponseTypes, L("CustomerServiceResponseTypes"));
            customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerServiceWorkTime, L("CustomerServiceWorkTime"));
            customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerServiceReports, L("CustomerServiceReports"));
            customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerServiceConversations, L("CustomerServiceConversations"));

            var customerMartial=customerService.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerMartial, L("CustomerMartial"));
            customerMartial.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaImages, L("CustomerMediaImages"));
            customerMartial.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaVoices, L("CustomerMediaVoices"));
            customerMartial.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaVideos, L("CustomerMediaVideos"));
            customerMartial.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerArticles, L("CustomerArticles"));
            customerMartial.CreateChildPermission(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerArticleGroups, L("CustomerArticleGroups"));


            pages.CreateChildPermission(AppPermissions.Pages_MpUserMembers, L("MpUserMembers"));

            pages.CreateChildPermission(AppPermissions.Pages_MpMenus, L("MpMenus"));
            pages.CreateChildPermission(AppPermissions.Pages_MpMessages, L("MpMessages"));
            pages.CreateChildPermission(AppPermissions.Pages_MpBabyText, L("MpBabyText"));
            var mpProduducts = pages.CreateChildPermission(AppPermissions.Pages_MpProducts, L("MpProducts"));
            mpProduducts.CreateChildPermission(AppPermissions.Pages_MpProducts_MpProductTypes, L("MpProductTypes"));
            mpProduducts.CreateChildPermission(AppPermissions.Pages_MpProducts_MpProductInfos, L("MpProductInfos"));

            pages.CreateChildPermission(AppPermissions.Pages_MpShoppingMallPic, L("MpShoppingMallPic"));
            var mpSolicitude=pages.CreateChildPermission(AppPermissions.Pages_MpSolicitude, L("MpSolicitude"));
            mpSolicitude.CreateChildPermission(AppPermissions.Pages_MpSolicitude_Settings, L("MpSolicitudeSetting"));
            mpSolicitude.CreateChildPermission(AppPermissions.Pages_MpSolicitude_Texts, L("MpSolicitudeText"));

            //var cy= pages.CreateChildPermission(AppPermissions.Pages_CY, L("ChunYuYiSheng"));
            //cy.CreateChildPermission(AppPermissions.Pages_CY_Configs, L("ChuYuConfigs"));
            //cy.CreateChildPermission(AppPermissions.Pages_CY_Problems, L("ChuYuProblems"));
            //cy.CreateChildPermission(AppPermissions.Pages_CY_ProblemContents, L("ChuYuProblemContents"));
            //cy.CreateChildPermission(AppPermissions.Pages_CY_Doctors, L("ChuYuDoctors"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroTemplateConsts.LocalizationSourceName);
        }
    }
}
