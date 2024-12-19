using Dishes.Common.Models;
using Dishes.Webhook.Data;
using Microsoft.EntityFrameworkCore;

namespace Dishes.Webhook.Repositories;

public class WebhookRepository : IWebhookRepository
{
    private readonly WebhookDbContext _context;

    public WebhookRepository(WebhookDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WebhookSubscription>> GetSubscriptionsAsync(string eventName)
    {
        return await _context.WebhookSubscriptions
                             .Where(ws => ws.Event == eventName)
                             .ToListAsync();
    }

    public async Task AddSubscriptionAsync(WebhookSubscription subscription)
    {
        subscription.CreatedAt = DateTime.UtcNow;
        _context.WebhookSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveSubscriptionAsync(int id)
    {
        var subscription = await _context.WebhookSubscriptions.FindAsync(id);
        if (subscription != null)
        {
            _context.WebhookSubscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<WebhookSubscription>> GetAllSubscriptionsAsync()
    {
        return await _context.WebhookSubscriptions.ToListAsync();
    }
}
