namespace Pulp.WebApi.Extensions.Generic.Models;

public class ContentGenerationHints
{
    public string? FakerHint { get; }
    public int? Min { get; }
    public int? Max { get; }
    public int? MinLength { get; }
    public int? MaxLength { get; }
    public IEnumerable<string>? AllowedValues { get; }

    public ContentGenerationHints(string? fakerHint,
        int? min,
        int? max,
        int? minLength,
        int? maxLength,
        IEnumerable<string>? allowedValues)
    {
        FakerHint = fakerHint;
        Min = min;
        Max = max;
        MinLength = minLength;
        MaxLength = maxLength;
        AllowedValues = allowedValues;
    }
}