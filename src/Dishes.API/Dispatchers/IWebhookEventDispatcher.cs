namespace Dishes.API.Dispatchers;

public interface IWebhookEventDispatcher
{
    Task DispatchEventAsync(string eventName, object payload);
}
