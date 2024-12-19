// Controllers/WebhookController.cs
using AutoMapper;
using Dishes.Common.Models;
using Dishes.Webhook.Data;
using Dishes.Webhook.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Dishes.Webhook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly IWebhookRepository _repository;
    private readonly ILogger<WebhookController> _logger;
    private readonly IMapper _mapper;

    public WebhookController(
        IWebhookRepository repository,
        ILogger<WebhookController> logger,
        IMapper mapper
    )
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    // Subscribe to an event
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(WebhookSubscriptionDTO subscription)
    {
        if (string.IsNullOrEmpty(subscription.Event))
            return BadRequest("Event and CallbackUrl are required.");

        var webhookSubscription = _mapper.Map<WebhookSubscription>(subscription);

        await _repository.AddSubscriptionAsync(webhookSubscription);
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

    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        var subscriptions = await _repository.GetAllSubscriptionsAsync();
        var webhookSubscriptions = _mapper.Map<List<WebhookSubscriptionDTO>>(subscriptions);
        return Ok(webhookSubscriptions);
    }

    [HttpGet("subscriptions/{eventName}")]
    public async Task<IActionResult> GetSubscription(string eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            return BadRequest("Event query parameter is required.");
        }

        var subscriptions = await _repository.GetSubscriptionsAsync(eventName);
        var webhookSubscriptions = _mapper.Map<List<WebhookSubscriptionDTO>>(subscriptions);
        return Ok(webhookSubscriptions);
    }
}
