using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Pb.Wechat.EntityFrameworkCore
{
    public static class KfDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<KfDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, b => b.UseRowNumberForPaging());
        }

        public static void Configure(DbContextOptionsBuilder<KfDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection, b => b.UseRowNumberForPaging());
        }
    }
}
