using Abp.Application.Navigation;
using Abp.Localization;
using Pb.Wechat.Authorization;

namespace Pb.Wechat.Web.Areas.AppAreaName.Startup
{
    public class AppAreaNameNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";
        public string appPath = AppConsts.MvcAppPath;

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Host.Tenants,
                        L("Tenants"),
                        url: appPath+"AppAreaName/Tenants",
                        icon: "icon-globe",
                        requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Host.Editions,
                        L("Editions"),
                        url: appPath+"AppAreaName/Editions",
                        icon: "icon-grid",
                        requiredPermissionName: AppPermissions.Pages_Editions
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.Administration,
                        L("Administration"),
                        icon: "icon-wrench"
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: appPath+"AppAreaName/OrganizationUnits",
                            icon: "icon-layers",
                            requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.Roles,
                            L("Roles"),
                            url: appPath+"AppAreaName/Roles",
                            icon: "icon-briefcase",
                            requiredPermissionName: AppPermissions.Pages_Administration_Roles
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.Users,
                            L("Users"),
                            url: appPath+"AppAreaName/Users",
                            icon: "icon-people",
                            requiredPermissionName: AppPermissions.Pages_Administration_Users
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.Languages,
                            L("Languages"),
                            url: appPath+"AppAreaName/Languages",
                            icon: "icon-flag",
                            requiredPermissionName: AppPermissions.Pages_Administration_Languages
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: appPath+"AppAreaName/AuditLogs",
                            icon: "icon-lock",
                            requiredPermissionName: AppPermissions.Pages_Administration_AuditLogs
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: appPath+"AppAreaName/Maintenance",
                            icon: "icon-wrench",
                            requiredPermissionName: AppPermissions.Pages_Administration_Host_Maintenance
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: appPath+"AppAreaName/SubscriptionManagement",
                            icon: "icon-refresh",
                            requiredPermissionName: AppPermissions.Pages_Administration_Tenant_SubscriptionManagement
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Host.Settings,
                            L("Settings"),
                            url: appPath+"AppAreaName/HostSettings",
                            icon: "icon-settings",
                            requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Tenant.Settings,
                            L("Settings"),
                            url: appPath+"AppAreaName/Settings",
                            icon: "icon-settings",
                            requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.Hangfire,
                        L("Hangfire"),
                        url: "hangfire",
                        icon: "icon-puzzle",
                        requiredPermissionName: AppPermissions.Pages_Administration_HangfireDashboard
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.MpManagers,
                        L("MpManagers"),
                        icon: "icon-event",
                        requiredPermissionName: AppPermissions.Pages_MpManagers
                    )
                    //.AddItem(new MenuItemDefinition(
                    //        AppAreaNamePageNames.Common.MpAccounts,
                    //        L("MpAccounts"),
                    //        url: appPath+"AppAreaName/MpAccounts",
                    //        requiredPermissionName: AppPermissions.Pages_MpManagers_MpAccounts
                    //    )
                    //)
                    .AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.MpApiTokens,
                        L("MpApiTokens"),
                        icon: "icon-empty",
                        url: appPath+"AppAreaName/MpApiTokens",
                        requiredPermissionName: AppPermissions.Pages_MpManagers_MpApiTokens
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpChannels,
                            L("MpChannels"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpChannels",
                            requiredPermissionName: AppPermissions.Pages_MpManagers_MpChannels
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.MaterialManage,
                        L("MaterialManage"),
                        icon: "icon-puzzle"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMediaImages,
                            L("MpMediaImages"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpMediaImages",
                            requiredPermissionName: AppPermissions.Pages_MaterialManage_MpMediaImages
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMediaVideos,
                            L("MpMediaVideos"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpMediaVideos",
                            requiredPermissionName: AppPermissions.Pages_MaterialManage_MpMediaVideos
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMediaVoices,
                            L("MpMediaVoices"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpMediaVoices",
                            requiredPermissionName: AppPermissions.Pages_MaterialManage_MpMediaVoices
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMediaArticles,
                            L("MpMediaArticles"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpMediaArticles",
                            requiredPermissionName: AppPermissions.Pages_MaterialManage_MpMediaArticles
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMediaArticleGroups,
                            L("MpMediaArticleGroups"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpMediaArticleGroups",
                            requiredPermissionName: AppPermissions.Pages_MaterialManage_MpMediaArticleGroups
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMediaImageTypes,
                            L("MpMediaImageTypes"),
                            icon: "icon-empty",
                            url: appPath + "AppAreaName/MpMediaImageTypes",
                            requiredPermissionName: AppPermissions.Pages_MaterialManage_MpMediaImageTypes
                        )
                    )
                 )
                .AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.MpUserManagement,
                        L("MpUserManagement"),
                        icon: "icon-bubbles"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpFansTags,
                            L("MpFansTags"),
                            icon: "icon-empty",
                            url: appPath+ "AppAreaName/MpFansTags",
                            requiredPermissionName: AppPermissions.Pages_MpUserManagement_MpFansTags
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpFans,
                            L("MpFans"),
                            icon: "icon-empty",
                            url: appPath + "AppAreaName/MpFans",
                            requiredPermissionName: AppPermissions.Pages_MpUserManagement_MpFans
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpGroups,
                            L("MpGroups"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpGroups",
                            requiredPermissionName: AppPermissions.Pages_MpUserManagement_MpGroups
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpUserMembers,
                            L("MpUserMembers"),
                            icon: "icon-empty",
                            url: appPath + "AppAreaName/MpUserMembers",
                            requiredPermissionName: AppPermissions.Pages_MpUserManagement_MpUserMembers
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.ReplyManagement,
                        L("ReplyManagement"),
                        icon: "icon-bubble"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpSelfArticles,
                            L("MpSelfArticles"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpSelfArticles",
                            requiredPermissionName: AppPermissions.Pages_ReplyManagement_MpSelfArticles
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpSelfArticleGroups,
                            L("MpSelfArticleGroups"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpSelfArticleGroups",
                            requiredPermissionName: AppPermissions.Pages_ReplyManagement_MpSelfArticleGroups
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpEvents,
                            L("MpEvents"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpEvents/Index",
                            requiredPermissionName: AppPermissions.Pages_ReplyManagement_MpEvents
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpKeyWordReplys,
                            L("MpKeyWordReplys"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpKeyWordReplys",
                            requiredPermissionName: AppPermissions.Pages_ReplyManagement_MpKeyWordReplys
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpFocus,
                            L("MpFoucus"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/MpEvents/RIndex",
                            requiredPermissionName: AppPermissions.Pages_ReplyManagement_MpFocus
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppAreaNamePageNames.Common.CustomerServiceOnline,
                        L("CustomerService"),
                        icon: "icon-earphones-alt",
                        requiredPermissionName: AppPermissions.Pages_CustomerService
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerServiceOnline,
                            L("CustomerServiceOnline"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/CustomerServiceOnlines",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceOnline
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerServiceResponseText,
                            L("CustomerServiceResponseText"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/CustomerServiceResponseTexts",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceResponseText
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerServiceWorkTime,
                            L("CustomerServiceWorkTime"),
                            icon: "icon-empty",
                            url: appPath+"AppAreaName/CustomerServiceWorkTimes",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceWorkTime
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerServiceResponseTypes,
                            L("CustomerServiceResponseTypes"),
                            icon: "icon-empty",
                            url: appPath + "AppAreaName/CustomerServiceResponseTypes",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceResponseTypes
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerServiceReports,
                            L("CustomerServiceReports"),
                            icon: "icon-empty",
                            url: appPath + "AppAreaName/CustomerServiceReports",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceReports
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerServiceConversations,
                            L("CustomerServiceConversations"),
                            icon: "icon-empty",
                            url: appPath + "AppAreaName/CustomerServiceConversations",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceConversations
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerMartial,
                            L("CustomerMartial"),
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerMartial
                        )
                        .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerCommonTexts,
                            L("CustomerCommonTexts"),
                            url: appPath + "AppAreaName/CustomerCommonTexts",
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerServiceResponseText
                            )
                        )
                         .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerMediaImages,
                            L("CustomerMediaImages"),
                            url: appPath + "AppAreaName/CustomerMediaImages",
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaImages
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerMediaVoices,
                            L("CustomerMediaVoices"),
                            url: appPath + "AppAreaName/CustomerMediaVoices",
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaVoices
                            )
                        )
                         .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerMediaVideos,
                            L("CustomerMediaVideos"),
                            url: appPath + "AppAreaName/CustomerMediaVideos",
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaVideos
                            )
                        )
                         .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerArticles,
                            L("CustomerArticles"),
                            url: appPath + "AppAreaName/CustomerArticles",
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerMartial_CustomerArticles
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.CustomerArticleGroups,
                            L("CustomerArticleGroups"),
                            url: appPath + "AppAreaName/CustomerArticleGroups",
                            icon: "icon-empty",
                            requiredPermissionName: AppPermissions.Pages_CustomerService_CustomerMartial_CustomerArticleGroups
                            )
                        )
                    )
                   
                )
                .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMenus,
                            L("MpMenus"),
                            url: appPath+"AppAreaName/MpMenus",
                            icon: "icon-menu",
                            requiredPermissionName: AppPermissions.Pages_MpMenus
                        )
                    )
                .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpSolicitude,
                            L("MpSolicitude"),
                            icon: "icon-eye",
                            requiredPermissionName: AppPermissions.Pages_MpSolicitude
                        ).AddItem(new MenuItemDefinition(
                                AppAreaNamePageNames.Common.MpSolicitudeSettings,
                                L("MpSolicitudeSetting"),
                                url: appPath + "AppAreaName/MpSolicitudeSettings",
                                icon: "icon-empty",
                                requiredPermissionName: AppPermissions.Pages_MpSolicitude_Settings
                            )
                        ).AddItem(new MenuItemDefinition(
                                AppAreaNamePageNames.Common.MpSolicitudeTexts,
                                L("MpSolicitudeText"),
                                url: appPath + "AppAreaName/MpSolicitudeTexts",
                                icon: "icon-empty",
                                requiredPermissionName: AppPermissions.Pages_MpSolicitude_Texts
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpMessages,
                            L("MpMessages"),
                            url: appPath+"AppAreaName/MpMessages",
                            icon: "icon-envelope-letter",
                            requiredPermissionName: AppPermissions.Pages_MpMessages
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpBabyText,
                            L("MpBabyText"),
                            url: appPath+"AppAreaName/MpBabyTexts",
                            icon: "icon-doc",
                            requiredPermissionName: AppPermissions.Pages_MpBabyText
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpShoppingMallPic,
                            L("MpShoppingMallPic"),
                            url: appPath+"AppAreaName/MpShoppingMallPics",
                            icon: "icon-picture",
                            requiredPermissionName: AppPermissions.Pages_MpShoppingMallPic
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppAreaNamePageNames.Common.MpProducts,
                            L("MpProducts"),
                            icon: "icon-doc",
                            requiredPermissionName: AppPermissions.Pages_MpProducts
                        )
                         .AddItem(new MenuItemDefinition(
                                AppAreaNamePageNames.Common.MpProductTypes,
                                L("MpProductTypes"),
                                url: appPath + "AppAreaName/MpProductTypes",
                                icon: "icon-doc",
                                requiredPermissionName: AppPermissions.Pages_MpProducts_MpProductTypes
                            )
                        )
                         .AddItem(new MenuItemDefinition(
                                AppAreaNamePageNames.Common.MpProductInfos,
                                L("MpProductInfos"),
                                url: appPath + "AppAreaName/MpProductInfos",
                                icon: "icon-doc",
                                requiredPermissionName: AppPermissions.Pages_MpProducts_MpProductInfos
                            )
                        )
                    )
            // .AddItem(new MenuItemDefinition(
            //        AppAreaNamePageNames.Common.CYManagment,
            //        L("ChuYuYiSheng"),
            //        icon: "icon-puzzle",
            //        requiredPermissionName: AppPermissions.Pages_CY
            //    )
            //    .AddItem(new MenuItemDefinition(
            //            AppAreaNamePageNames.Common.CYConfigs,
            //            L("ChuYuConfigs"),
            //            url: appPath+"AppAreaName/CYConfigs",
            //            requiredPermissionName: AppPermissions.Pages_CY_Configs
            //        )
            //    )
            //    .AddItem(new MenuItemDefinition(
            //            AppAreaNamePageNames.Common.CYProblems,
            //            L("ChuYuProblems"),
            //            url: appPath+"AppAreaName/CYProblems",
            //            requiredPermissionName: AppPermissions.Pages_CY_Problems
            //        )
            //    )
            //    //.AddItem(new MenuItemDefinition(
            //    //        AppAreaNamePageNames.Common.CYProblems,
            //    //        L("ChuYuProblems"),
            //    //        url: appPath+"AppAreaName/CYProblems",
            //    //        requiredPermissionName: AppPermissions.Pages_CY_Problems
            //    //    )
            //    //)
            //    .AddItem(new MenuItemDefinition(
            //        AppAreaNamePageNames.Common.CYDoctors,
            //        L("ChuYuDoctors"),
            //        url: appPath+"AppAreaName/CYDoctors",
            //        requiredPermissionName: AppPermissions.Pages_CY_Doctors
            //    )
            //)
            //)
            ;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroTemplateConsts.LocalizationSourceName);
        }
    }
}