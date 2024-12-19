using System.Reflection.Metadata;

namespace Dishes.Webhook;

public class WebhookPayload
{
    public string Event { get; set; }
    public Document? Data { get; set; }
}
