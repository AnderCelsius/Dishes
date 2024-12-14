namespace Dishes.Common.Models;

public class WebhookSubscription
{
    public int Id { get; set; }
    public string Event { get; set; } // e.g., "DishAdded", "DishDeleted", "IngredientsAdded"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

