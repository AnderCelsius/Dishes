using Dishes.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Dishes.Common.Configurations.Serialization;

public class JsonDocumentResponseConverter : JsonConverter<JsonDocument>
{
    public override JsonDocument? ReadJson(
        JsonReader reader,
        Type objectType,
        JsonDocument? existingValue,
        bool hasExistingValue,
        Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject)
        {
            var jObject = JObject.Load(reader);
            return JsonDocument.Parse(jObject.ToString());
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, JsonDocument? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        var rawJsonValue = value?.ToJsonString();
        writer.WriteRawValue(rawJsonValue ?? "{}");
    }
}
