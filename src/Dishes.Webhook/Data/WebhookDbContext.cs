using Dishes.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Dishes.Webhook.Data;

public class WebhookDbContext : DbContext
{
    public WebhookDbContext(DbContextOptions<WebhookDbContext> options) : base(options) { }

    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
}