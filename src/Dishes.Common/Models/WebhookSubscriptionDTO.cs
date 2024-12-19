using Dishes.Common.Configurations.Serialization;
using Newtonsoft.Json;
using System.Text.Json;

namespace Dishes.Common.Models;

public class WebhookSubscriptionDTO
{
    public string Event { get; set; } // e.g., "DishAdded", "DishDeleted", "IngredientsAdded"

    [JsonConverter(typeof(JsonDocumentResponseConverter))]
    public JsonDocument? Data { get; set; }
}

