using Microsoft.OpenApi.Models;
using Pulp.WebApi.Extensions.Generic.Models;

namespace Pulp.WebApi.Import.OpenApi.Converters;

internal static class OpenApiPathConverter
{
    internal static IEnumerable<DynamicEndpoint> ToEndpoints(string routePattern, OpenApiPathItem pathItem)
    {
        foreach (var kvp in pathItem.Operations)
        {
            yield return OpenApiOperationConverter.ToBackendEndpoint(kvp.Key, routePattern, kvp.Value);
        }
    }
}
