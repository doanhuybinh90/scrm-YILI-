using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Pb.Wechat.EntityFrameworkCore
{
    public static class GroupMessageDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<GroupMessageDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, b => b.UseRowNumberForPaging());
        }

        public static void Configure(DbContextOptionsBuilder<GroupMessageDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection, b => b.UseRowNumberForPaging());
        }
    }
}
