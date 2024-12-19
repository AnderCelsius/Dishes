using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace Dishes.Common.Configurations.Serialization;

public class JsonOptionsConfigurationExtensions :
    IConfigureOptions<Microsoft.AspNetCore.Mvc.JsonOptions>,
    IConfigureOptions<JsonOptions>
{
    public void Configure(JsonOptions options) =>
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

    public void Configure(Microsoft.AspNetCore.Mvc.JsonOptions options) =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}
