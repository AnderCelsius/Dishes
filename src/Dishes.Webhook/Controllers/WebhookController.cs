using Microsoft.AspNetCore.Mvc;

namespace Dishes.Webhook.Controllers;

[ApiController]
[Route("[controller]")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;

    // Replace with your actual secret token for security
    private readonly string _secretToken = "YourSecretTokenHere";

    public WebhookController(ILogger<WebhookController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Receive([FromBody] WebhookPayload payload)
    {
        // Verify the secret token
        if (!Request.Headers.TryGetValue("X-Webhook-Secret", out var extractedToken))
        {
            _logger.LogWarning("Missing X-Webhook-Secret header.");
            return Unauthorized();
        }

        if (extractedToken != _secretToken)
        {
            _logger.LogWarning("Invalid secret token.");
            return Unauthorized();
        }

        // Log the received payload
        _logger.LogInformation("Received webhook payload: {@Payload}", payload);

        // TODO: Add your processing logic here
        // For example, update the database, trigger business logic, etc.

        // Respond with 200 OK to acknowledge receipt
        return Ok(new { status = "Webhook received successfully" });
    }
}
