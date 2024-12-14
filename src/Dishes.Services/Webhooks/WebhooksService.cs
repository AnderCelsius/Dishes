using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dishes.Core.Contracts;
using Dishes.Core.Events;

namespace Dishes.API.Services;

public class WebhookService : IWebhookService
{
    private readonly ILogger<WebhookService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    // List of webhook URLs to notify
    private readonly List<string> _webhookUrls;

    public WebhookService(
        ILogger<WebhookService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;

        // Load webhook URLs from configuration
        _webhookUrls = _configuration.GetSection("Webhooks:Urls").Get<List<string>>() ?? new List<string>();
    }

    public async Task SendWebhookAsync(WebhookEvent webhookEvent, object data)
    {
        var payload = new
        {
            @event = webhookEvent.ToString(),
            data = data
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient();

        foreach (var url in _webhookUrls)
        {
            try
            {
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully sent webhook to {Url}", url);
                }
                else
                {
                    _logger.LogWarning("Failed to send webhook to {Url}. Status Code: {StatusCode}", url, response.StatusCode);
                    // Optionally, implement retry logic or error handling here
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending webhook to {Url}", url);
                // Optionally, implement retry logic or error handling here
            }
        }
    }

}
