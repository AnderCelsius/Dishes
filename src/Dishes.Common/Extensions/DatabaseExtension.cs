using Dishes.Common.Configurations;
using Dishes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dishes.Common.Extensions;

public static class DatabaseExtension
{
    public static void AddDatabase<TDbContext>(
        this IServiceCollection serviceCollection,
        DatabaseConfiguration? databaseConfiguration)
        where TDbContext : DbContext
    {
        serviceCollection.AddDbContextFactory<TDbContext>(builder => builder.UseSqlite(databaseConfiguration?.ConnectionString));

        serviceCollection.AddTransient<DatabaseMigrator<TDbContext>>();
    }

    public static Task MigrateDatabaseToLatestVersion<TDbContext>(this IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        return serviceProvider.GetRequiredService<DatabaseMigrator<TDbContext>>().MigrateDbToLatestVersion();
    }
}