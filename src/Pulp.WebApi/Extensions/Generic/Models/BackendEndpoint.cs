namespace Pulp.WebApi.Extensions.Generic.Models;

public abstract class BackendEndpoint
{
    public string Method { get; }
    public string RoutePattern { get; }

    protected BackendEndpoint(string method,
        string routePattern)
    {
        Method = method;
        RoutePattern = routePattern;
    }
}
