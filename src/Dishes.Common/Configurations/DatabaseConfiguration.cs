namespace Dishes.Common.Configurations;

public record DatabaseConfiguration
{
    public string? ConnectionString { get; init; }
}

