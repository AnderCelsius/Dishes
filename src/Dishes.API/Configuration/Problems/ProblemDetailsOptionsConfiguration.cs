using Dishes.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace Dishes.API.Configuration.Problems;

public class ProblemDetailsOptionsConfiguration : IConfigureOptions<ProblemDetailsOptions>
{
    private readonly IWebHostEnvironment _env;

    public ProblemDetailsOptionsConfiguration(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void Configure(ProblemDetailsOptions options)
    {
        options.IncludeExceptionDetails = (_, _) => _env.IsDevelopment();

        options.Map<BaseException>((ctx, exception) =>
        {
            return new ProblemDetails
            {
                Title = exception.Title,
                Detail = exception.Detail,
                Status = exception.Status,
                Type = exception.ExceptionType,
                Instance = ctx.Request.Path.Value,
            };
        });

        options.Map<ValidationException>((ctx, exception) =>
        {
            return new ProblemDetails
            {
                Title = "Validation Exception",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest,
                Instance = ctx.Request.Path.Value,
            };
        });

        options.Map<ODataException>((ctx, exception) =>
        {
            return new ProblemDetails
            {
                Title = "OData Exception",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest,
                Instance = ctx.Request.Path.Value,
            };
        });
    }
}
