using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dishes.Common.Configurations.Serialization;

public class NewtonSoftJsonSerializerOptionsConfigurator : IConfigureOptions<MvcNewtonsoftJsonOptions>
{
    public void Configure(MvcNewtonsoftJsonOptions options)
    {
        options.SerializerSettings.DateFormatHandling = NewtonSoftJsonSerializerSettings.Default.DateFormatHandling;
        options.SerializerSettings.DateTimeZoneHandling = NewtonSoftJsonSerializerSettings.Default.DateTimeZoneHandling;
        options.SerializerSettings.ReferenceLoopHandling = NewtonSoftJsonSerializerSettings.Default.ReferenceLoopHandling;
        options.SerializerSettings.ContractResolver = NewtonSoftJsonSerializerSettings.Default.ContractResolver;
        options.SerializerSettings.Converters = NewtonSoftJsonSerializerSettings.Default.Converters;
    }
}
