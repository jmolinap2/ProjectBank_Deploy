using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ProjectBlack.EntityFrameworkCore;

public static class ProjectBlackDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<ProjectBlackDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<ProjectBlackDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
