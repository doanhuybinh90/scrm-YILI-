using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.CustomerServiceConversations;
using Pb.Wechat.CustomerServiceConversationMsgs;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.CustomerServicePrivateResponseTexts;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.CustomerServiceWorkTimes;
using Pb.Wechat.CustomerServiceResponseTypes;
using Pb.Wechat.CustomerInOutLogs;
using Pb.Wechat.CustomerMediaImages;
using Pb.Wechat.CustomerMediaVoices;
using Pb.Wechat.CustomerMediaVideos;
using Pb.Wechat.CustomerArticles;
using Pb.Wechat.CustomerArticleGroups;
using Pb.Wechat.CustomerArticleGroupItems;
using Pb.Wechat.CustomerServiceReports;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class KfDbContext : AbpDbContext
    {
        public DbSet<CustomerServiceOnline> CustomerServiceOnlines { get; set; }
        public DbSet<CustomerServiceResponseText> CustomerServiceResponseTexts { get; set; }
        public DbSet<CustomerServiceWorkTime> CustomerServiceWorkTimes { get; set; }
        public DbSet<CustomerServicePrivateResponseText> CustomerServicePrivateResponseTexts { get; set; }
        public DbSet<CustomerServiceConversationMsg> CustomerServiceConversationMsgs { get; set; }
        public DbSet<CustomerServiceConversation> CustomerServiceConversations { get; set; }
        public DbSet<CustomerServiceResponseType> CustomerServiceResponseTypes { get; set; }
        public DbSet<CustomerInOutLog> CustomerInOutLogs { get; set; }

        public DbSet<CustomerMediaImage> CustomerMediaImages { get; set; }
        public DbSet<CustomerMediaVoice> CustomerMediaVoices { get; set; }
        public DbSet<CustomerMediaVideo> CustomerMediaVideos { get; set; }
        public DbSet<CustomerArticle> CustomerArticles { get; set; }
        public DbSet<CustomerArticleGroup> CustomerArticleGroups { get; set; }
        public DbSet<CustomerArticleGroupItem> CustomerArticleGroupItems { get; set; }
        public DbSet<CustomerServiceReport> CustomerServiceReports { get; set; }
        public KfDbContext(DbContextOptions<KfDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
        }
    }
}
