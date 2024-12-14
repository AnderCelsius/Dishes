using Dishes.Common.Models;

namespace Dishes.API.Dispatchers;

public class WebhookEventDispatcher : IWebhookEventDispatcher
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WebhookEventDispatcher> _logger;
    private readonly IConfiguration _configuration;

    public WebhookEventDispatcher(
        IHttpClientFactory httpClientFactory,
        ILogger<WebhookEventDispatcher> logger,
        IConfiguration configuration
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task DispatchEventAsync(string eventName, object payload)
    {
        var client = _httpClientFactory.CreateClient("WebhookService");
        var response = await client.GetAsync($"subscriptions?eventName={eventName}");

        if (response.IsSuccessStatusCode)
        {
            var subscriptions = await response.Content.ReadFromJsonAsync<IEnumerable<WebhookSubscription>>();
            string callbackUrl = _configuration.GetRequiredSection("WebhookOptions").Get<WebhooksOptions>().CallbackUrl;
            foreach (var subscription in subscriptions)
            {
                try
                {
                    var webhookClient = new HttpClient();
                    var httpResponse = await webhookClient.PostAsJsonAsync(callbackUrl, new
                    {
                        Event = eventName
                    });

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Failed to send webhook to {callbackUrl}. Status Code: {httpResponse.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception when sending webhook to {callbackUrl}: {ex.Message}");
                }
            }
        }
        else
        {
            _logger.LogError($"Failed to retrieve subscriptions for event {eventName}. Status Code: {response.StatusCode}");
        }
    }
}

