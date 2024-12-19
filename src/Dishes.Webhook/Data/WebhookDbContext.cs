using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

namespace Dishes.Webhook.Data;

public class WebhookDbContext : DbContext
{
    public WebhookDbContext(DbContextOptions<WebhookDbContext> options) : base(options) { }

    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebhookSubscription>()
            .Property(e => e.Data)
            .HasConversion(
                v => v == null ? null : v.ToString(), // Convert JObject to JSON string
                v => v == null ? null : JsonConvert.DeserializeObject<JsonDocument>(v) // Convert JSON string to JsonDocument
            )
            .HasColumnType("TEXT");
    }

}