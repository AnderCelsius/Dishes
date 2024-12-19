using System.Text.Json;

namespace Dishes.Webhook.Data;

public class WebhookSubscription
{
    public int Id { get; set; }

    public string Event { get; set; } // e.g., "DishAdded", "DishDeleted", "IngredientsAdded"

    public JsonDocument? Data { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

