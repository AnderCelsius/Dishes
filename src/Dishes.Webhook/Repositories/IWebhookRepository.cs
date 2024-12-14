using Dishes.Common.Models;

namespace Dishes.Webhook.Repositories;

public interface IWebhookRepository
{
    Task<IEnumerable<WebhookSubscription>> GetAllSubscriptionsAsync();
    Task<IEnumerable<WebhookSubscription>> GetSubscriptionsAsync(string eventName);
    Task AddSubscriptionAsync(WebhookSubscription subscription);
    Task RemoveSubscriptionAsync(int id);
}
