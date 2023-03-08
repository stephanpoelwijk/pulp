namespace Pulp.WebApi.Extensions.Generic.Models;

public class StaticEndpoint : BackendEndpoint
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
