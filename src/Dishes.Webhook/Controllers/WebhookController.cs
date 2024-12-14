// Controllers/WebhookController.cs
using Dishes.Common.Models;
using Dishes.Webhook.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Dishes.Webhook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly IWebhookRepository _repository;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(IWebhookRepository repository, ILogger<WebhookController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // Subscribe to an event
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] WebhookSubscription subscription)
    {
        if (string.IsNullOrEmpty(subscription.Event))
            return BadRequest("Event and CallbackUrl are required.");

        await _repository.AddSubscriptionAsync(subscription);
        _logger.LogInformation($"New event: {subscription.Event}");
        return Ok(new { message = "Subscription added successfully." });
    }

    // Unsubscribe from an event
    [HttpDelete("unsubscribe/{id}")]
    public async Task<IActionResult> Unsubscribe(int id)
    {
        await _repository.RemoveSubscriptionAsync(id);
        return Ok(new { message = "Subscription removed successfully." });
    }

    // (Optional) Get all subscriptions
    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        var subscriptions = await _repository.GetAllSubscriptionsAsync();
        return Ok(subscriptions);
    }

    [HttpGet("subscriptions/{eventName}")]
    public async Task<IActionResult> GetSubscription(string eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            return BadRequest("Event query parameter is required.");
        }

        var subscriptions = await _repository.GetSubscriptionsAsync(eventName);
        return Ok(subscriptions);
    }
}
