using Dishes.Common.Models;
using System.Text.Json;

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

        string jsonString = JsonSerializer.Serialize(payload);
        var subscription = new WebhookSubscriptionDTO
        {
            Event = eventName,
            Data = JsonDocument.Parse(jsonString)
        };

        string callbackUrl = _configuration.GetRequiredSection("WebhookOptions").Get<WebhooksOptions>().CallbackUrl;

        try
        {
            var webhookClient = new HttpClient();
            var httpResponse = await webhookClient.PostAsJsonAsync(callbackUrl, subscription);

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

