namespace Pulp.WebApi.Extensions.Generic.Models;

public class BackendService
{
    public string Name { get; }
    public IEnumerable<BackendEndpoint> Endpoints { get; } = Enumerable.Empty<BackendEndpoint>();

    public BackendService(string name,
        IEnumerable<BackendEndpoint> endpoints)
    {
        Name = name;
        Endpoints = endpoints;
    }
}
