using Microsoft.OpenApi.Models;
using Pulp.WebApi.Extensions.Generic.Models;

namespace Pulp.WebApi.Import.OpenApi.Converters;

internal static class OpenApiOperationConverter
{
    internal static DynamicEndpoint ToBackendEndpoint(OperationType operationType, string routePattern, OpenApiOperation operation)
    {
        OpenApiSchema? responseSchema = null;

        if (operation.Responses.TryGetValue("200", out var okResponse))
        {
            // Looks like the library fakes this Content property by combining
            // the operation.produces with the operation.responses property
            // 
            // That would mean that it doesn't really matter which response is picked here
            // because the schemas for each of the content types is the same
            if (okResponse.Content.TryGetValue("application/json", out var appJsonResponse))
            {
                responseSchema = appJsonResponse.Schema;
            }
        }

        return new DynamicEndpoint(operationType.ToString(), routePattern, OpenApiSchemaConverter.ToTypeSchema(responseSchema));
    }
}
