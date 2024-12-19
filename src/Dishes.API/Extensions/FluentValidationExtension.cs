using FluentValidation;
using FluentValidation.AspNetCore;
using static Dishes.API.Features.Dishes.Create;

namespace Dishes.API.Extensions;

public static class FluentValidationExtension
{
    public static void AddFluentValidationExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<CreateDishValidator>();
    }
}
