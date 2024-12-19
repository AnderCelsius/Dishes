using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Dishes.Common.Configurations.Serialization;

public class NewtonSoftJsonSerializerSettings
{
    public static readonly JsonSerializerSettings Default = CreateDefault();

    private static JsonSerializerSettings CreateDefault()
    {
        return new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
            Converters =
            {
                new StringEnumConverter(),
                new JsonDocumentResponseConverter()
            }
        };
    }
}
