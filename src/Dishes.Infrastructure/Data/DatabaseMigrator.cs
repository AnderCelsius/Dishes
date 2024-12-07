using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dishes.Infrastructure.Data;

public class DatabaseMigrator<TDbContext>(IDbContextFactory<TDbContext> contextFactory,
  ILogger<DatabaseMigrator<TDbContext>> logger)
  where TDbContext : DbContext
{
  public async Task MigrateDbToLatestVersion()
  {
    await using var context = await contextFactory.CreateDbContextAsync();
    context.Database.SetCommandTimeout(300);

    await using var dbConnection = context.Database.GetDbConnection();
    logger.LogInformation(
      "Migrating standards database {SQL_DATABASE} on server {SQL_LITE}...",
      dbConnection.Database,
      dbConnection.DataSource);

    await context.Database.MigrateAsync();

    logger.LogInformation("Database migrated successfully");
  }
}
