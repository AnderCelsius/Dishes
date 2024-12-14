using Dishes.Core.Events;

namespace Dishes.Core.Contracts;

public interface IWebhookService
{
    Task SendWebhookAsync(WebhookEvent webhookEvent, object data);
}
