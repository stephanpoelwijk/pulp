namespace Pulp.WebApi.Extensions.Generic.Models;

public class DynamicEndpoint : BackendEndpoint
{
    public TypeSchema? ResponseTypeSchema { get; }

    public DynamicEndpoint(string method,
        string routePattern,
        TypeSchema? responseTypeSchema) : base(method, routePattern)
    {
        ResponseTypeSchema = responseTypeSchema;
    }
}
