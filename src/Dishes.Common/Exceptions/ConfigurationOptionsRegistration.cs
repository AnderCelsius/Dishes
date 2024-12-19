using Dishes.Common.Configurations.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Dishes.API.Extensions;

public static class ConfigurationOptionsRegistration
{
    private static readonly Type[] _optionTypes =
    [
        typeof(IConfigureOptions<>),
        typeof(IPostConfigureOptions<>),
        typeof(IValidateOptions<>),
    ];

    public static void AddCommonConfigurationOptions(this IServiceCollection services)
    {
        IEnumerable<TypeInfo> concreteOptionTypes = typeof(NewtonSoftJsonSerializerOptionsConfigurator).Assembly.DefinedTypes.Where(
            x => x.ImplementedInterfaces.Any(y => y.IsGenericType && _optionTypes.Contains(y.GetGenericTypeDefinition()))
        );

        foreach (TypeInfo optionType in concreteOptionTypes)
        {
            services.ConfigureOptions(optionType);
        }
    }
}
