namespace Pulp.WebApi.Extensions.Generic;

public class BackendService
{
    public string Name { get; }
    public IEnumerable<Endpoint> Endpoints { get; } = Enumerable.Empty<Endpoint>();

    public BackendService(string name, 
        IEnumerable<Endpoint> endpoints)
    {
        Name = name;
        Endpoints = endpoints;
    }
}

public abstract class Endpoint
{
    public string Method { get; }
    public string RoutePattern { get; }

    protected Endpoint(string method, 
        string routePattern)
    {
        Method = method;
        RoutePattern = routePattern;
    }
}

public class DynamicEndpoint : Endpoint
{
    public TypeSchema ResponseTypeSchema { get; }

    public DynamicEndpoint(string method, 
        string routePattern,
        TypeSchema responseTypeSchema) : base(method, routePattern)
    {
        ResponseTypeSchema = responseTypeSchema;
    }
}

public class StaticEndpoint: Endpoint
{
    public string ContentType { get; }
    public string ResponseContents { get; }

    public StaticEndpoint(string method,
        string routePattern,
        string contentType,
        string responseContents) : base(method, routePattern)
    {
        ContentType = contentType;
        ResponseContents = responseContents;
    }
}

public class TypeSchema
{
    public static TypeSchema Integer = new(GenerationType.Int, Enumerable.Empty<Property>(), null);
    public static TypeSchema String = new(GenerationType.String, Enumerable.Empty<Property>(), null);

    public GenerationType Type { get; }
    public ContentGenerationHints? ContentGenerationHints { get; }
    public IEnumerable<Property> Properties { get; } = Enumerable.Empty<Property>();

    public TypeSchema(GenerationType type, 
        IEnumerable<Property> properties,
        ContentGenerationHints? contentGenerationHints)
    {
        Type = type;
        Properties = properties;
        ContentGenerationHints = contentGenerationHints;
    }
}

public enum GenerationType
{
    Int,
    Float,
    Long,
    String,
    Object
}

public class Property
{
    public string Name { get; } = default!;
    public TypeSchema TypeSchema { get; } = default!;
    public bool IsOptional { get; }

    public Property(TypeSchema typeSchema, 
        string name,
        bool isOptional)
    {
        Name = name;
        TypeSchema = typeSchema;
        IsOptional = isOptional;
    }
}

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