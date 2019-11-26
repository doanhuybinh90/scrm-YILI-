using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Pb.Wechat.EntityFrameworkCore
{
    public static class CYDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CYDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, b => b.UseRowNumberForPaging());
        }

        public static void Configure(DbContextOptionsBuilder<CYDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection, b => b.UseRowNumberForPaging());
        }
    }
}
