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

    public static void AddConfigurationOptions(this IServiceCollection services)
    {
        IEnumerable<TypeInfo> concreteOptionTypes = typeof(Program).Assembly.DefinedTypes.Where(
            x => x.ImplementedInterfaces.Any(y => y.IsGenericType && _optionTypes.Contains(y.GetGenericTypeDefinition()))
        );

        foreach (TypeInfo optionType in concreteOptionTypes)
        {
            services.ConfigureOptions(optionType);
        }
    }
}
