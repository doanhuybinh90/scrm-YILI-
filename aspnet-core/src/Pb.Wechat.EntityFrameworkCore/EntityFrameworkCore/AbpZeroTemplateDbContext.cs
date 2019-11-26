using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.Authorization.Roles;
using Pb.Wechat.Authorization.Users;
using Pb.Wechat.Chat;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.CustomerServiceWorkTimes;
using Pb.Wechat.Editions;
using Pb.Wechat.Friendships;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpApiTokens;
using Pb.Wechat.MpArticleInsideImages;
using Pb.Wechat.MpBabyTexts;
using Pb.Wechat.MpChannels;
using Pb.Wechat.MpCourseSignups;
using Pb.Wechat.MpEventClickViewLogs;
using Pb.Wechat.MpEventRequestMsgLogs;
using Pb.Wechat.MpEvents;
using Pb.Wechat.MpEventScanLogs;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpFansGroupMaps;
using Pb.Wechat.MpFansTagItems;
using Pb.Wechat.MpFansTags;
using Pb.Wechat.MpGroupItems;
using Pb.Wechat.MpGroups;
using Pb.Wechat.MpKeyWordReplys;
using Pb.Wechat.MpMediaArticleGroups;
using Pb.Wechat.MpMediaArticles;
using Pb.Wechat.MpMediaImages;
using Pb.Wechat.MpMediaImageTypes;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.MpMediaVoices;
using Pb.Wechat.MpMenus;
using Pb.Wechat.MpMessages;
using Pb.Wechat.MpProductInfos;
using Pb.Wechat.MpProductTypes;
using Pb.Wechat.MpSecondKeyWordReplys;
using Pb.Wechat.MpSelfArticleGroupItems;
using Pb.Wechat.MpSelfArticleGroups;
using Pb.Wechat.MpSelfArticles;
using Pb.Wechat.MpShoppingMallPics;
using Pb.Wechat.MpSolicitudeSettings;
using Pb.Wechat.MpSolicitudeTexts;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.MultiTenancy;
using Pb.Wechat.MultiTenancy.Payments;
using Pb.Wechat.Storage;
using Pb.Wechat.UserMps;
using Pb.Wechat.YiliLastBuyProducts;
using Pb.Wechat.YiliMemberTypes;
using Pb.Wechat.YiliOfficialCitys;
using Pb.Wechat.YiliOrganizeCitys;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class AbpZeroTemplateDbContext : AbpZeroDbContext<Tenant, Role, User, AbpZeroTemplateDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public DbSet<MpAccount> MpAccounts { get; set; }
        public DbSet<UserMp> UserMps { get; set; }
        public DbSet<MpApiToken> MpApiTokens { get; set; }
        public DbSet<MpEvent> MpEvents { get; set; }
        public DbSet<MpMediaVideo> MpMediaVideos { get; set; }
        public DbSet<MpSelfArticle> MpSelfArticles { get; set; }
        public DbSet<MpSelfArticleGroup> MpSelfArticleGroups { get; set; }
        public DbSet<MpSelfArticleGroupItem> MpSelfArticleGroupItems { get; set; }
        public DbSet<MpMediaImage> MpMediaImages { get; set; }
        public DbSet<MpMediaVoice> MpMediaVoices { get; set; }
        public DbSet<MpMediaArticle> MpMediaArticles { get; set; }
        public DbSet<MpMediaArticleGroup> MpMediaArticleGroups { get; set; }
        public DbSet<MpMediaArticleGroupItem> MpMediaArticleGroupItems { get; set; }
        public DbSet<MpUserMember> MpUserMembers { get; set; }
        public DbSet<MpFan> MpFans { get; set; }
        public DbSet<MpFansTag> MpFansTags { get; set; }
        public DbSet<MpFansTagItem> MpFansTagItems { get; set; }
        public DbSet<MpGroup> MpGroups { get; set; }
        public DbSet<MpChannel> MpChannels { get; set; }
        public DbSet<MpKeyWordReply> MpKeyWordReplys { get; set; }
        public DbSet<MpSecondKeyWordReply> MpSecondKeyWordReplys { get; set; }
        public DbSet<MpMenu> MpMenus { get; set; }
        public DbSet<MpMessage> MpMessages { get; set; }
        public DbSet<MpEventRequestMsgLog> MpEventRequestMsgLogs { get; set; }
        public DbSet<MpEventClickViewLog> MpEventClickViewLogs { get; set; }
        public DbSet<MpEventScanLog> MpEventScanLogs { get; set; }
        public DbSet<MpArticleInsideImage> MpArticleInsideImages { get; set; }
        //public DbSet<CustomerServiceOnline> CustomerServiceOnlines { get; set; }
        public DbSet<MpBabyText> MpBabyTexts { get; set; }
        //public DbSet<CustomerServiceResponseText> CustomerServiceResponseTexts { get; set; }
        public DbSet<MpShoppingMallPic> MpShoppingMallPics { get; set; }
        public DbSet<MpFansGroupMap> MpFansGroupMaps { get; set; }
        //public DbSet<CustomerServiceWorkTime> CustomerServiceWorkTimes { get; set; }
        public DbSet<MpGroupItem> MpGroupItems { get; set; }
        public DbSet<YiliLastBuyProduct> YiliLastBuyProducts { get; set; }
        public DbSet<YiliOfficialCity> YiliOfficialCitys { get; set; }
        public DbSet<YiliOrganizeCity> YiliOrganizeCitys { get; set; }
        public DbSet<YiliMemberType> YiliMemberTypes { get; set; }
        public DbSet<MpMediaImageType> MpMediaImageTypes { get; set; }
        public DbSet<MpProductInfo> MpProductInfos { get; set; }
        public DbSet<MpProductType> MpProductTypes { get; set; }
        public DbSet<MpCourseSignup> MpCourseSignups { get; set; }
        public DbSet<MpSolicitudeText> MpSolicitudeTexts { get; set; }
        public DbSet<MpSolicitudeSetting> MpSolicitudeSettings { get; set; }
        public AbpZeroTemplateDbContext(DbContextOptions<AbpZeroTemplateDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { e.PaymentId, e.Gateway });
            });

            modelBuilder.Entity<MpAccount>(b =>
            {
                b.HasIndex(e => new { e.Name, e.AccountType, e.Remark, e.AppId });
            });

            modelBuilder.Entity<MpApiToken>(b =>
            {
                b.HasIndex(e => new { e.ParentId, e.ApiType, e.Token, e.StartTime, e.EndTime });
            });

            modelBuilder.Entity<MpKeyWordReply>(b =>
            {
                b.HasIndex(e => new { e.KeyWord, e.MpID });
            });

            modelBuilder.Entity<MpSecondKeyWordReply>(b =>
            {
                b.HasIndex(e => new { e.KeyWord, e.ParentId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
