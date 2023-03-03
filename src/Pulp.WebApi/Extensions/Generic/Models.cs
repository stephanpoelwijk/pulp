namespace Pulp.WebApi.Extensions.Generic;

public class BackendService
{
    public string Name { get; }
    public IEnumerable<Endpoint> Endpoints { get; } = Enumerable.Empty<Endpoint>();
}

public class Endpoint
{
    public string Method { get; }
    public string RoutePattern { get; }
}

public class DynamicEndpoint : Endpoint
{
    public ResponseType ResponseType { get; }
}

public class StaticEndpoint: Endpoint
{
    public string ContentType { get; }
    public string ResponseContents { get; }
}

public class ResponseType
{
    public IEnumerable<Property> Properties { get; } = Enumerable.Empty<Property>();
}

public class Property
{
    public string Type { get; } = default!;
    public string Name { get; } = default!;
    public bool IsOptional { get; }

    public ContentGenerationHints? ContentGenerationHints { get; }
}

public class ContentGenerationHints
{
    public string? FakerHint { get; }
    public int? Min { get; }
    public int? Max { get; }
    public int? MinLength { get; }
    public int? MaxLength { get; }
}