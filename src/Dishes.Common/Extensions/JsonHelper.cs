using System.Text;
using System.Text.Json;

namespace Dishes.Common.Extensions;

public static class JsonHelper
{
    public static string ToJsonString(this JsonDocument? jsonDocument, bool indented)
    {
        if (jsonDocument == null)
        {
            return string.Empty;
        }

        using var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = indented });
        jsonDocument.WriteTo(writer);
        writer.Flush();

        var jsonString = Encoding.UTF8.GetString(stream.ToArray());
        return jsonString;
    }

    public static string ToJsonString(this JsonDocument jsonDocument) => jsonDocument.ToJsonString(indented: true);

    public static JsonSerializerOptions? GetJsonSettings()
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        return settings;
    }
}
