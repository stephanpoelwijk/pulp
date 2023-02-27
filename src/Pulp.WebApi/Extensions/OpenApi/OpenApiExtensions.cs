﻿using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Pulp.WebApi.Extensions.OpenApi;

public static class OpenApiExtensions
{
    public static IEndpointRouteBuilder UseOpenApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var configuration = builder.ServiceProvider.GetRequiredService<OpenApiConfiguration>();
        if (!File.Exists(configuration.SpecFileName))
        {
            throw new FileNotFoundException($"Could not find OpenApi spec '{configuration.SpecFileName}'", configuration.SpecFileName);
        }

        var objectGenerator = builder.ServiceProvider.GetRequiredService<IObjectGenerator>();

        using var stream = new FileStream(configuration.SpecFileName, FileMode.Open, FileAccess.Read);
        var document = new OpenApiStreamReader().Read(stream, out var diagnostic);

        foreach (var kvp in document.Paths)
        {
            var url = kvp.Key;
            var pathItem = kvp.Value;

            foreach (var operationKvp in pathItem.Operations)
            {
                var operationType = operationKvp.Key;
                var operation = operationKvp.Value;

                switch (operationType)
                {
                    case OperationType.Get:
                        builder.MapGet(url, () =>
                        {
                            object? response = null;
                            if (operation.Responses.TryGetValue("200", out var okResponse))
                            {
                                if (okResponse.Content.TryGetValue("application/json", out var appJsonResponse))
                                {
                                    response = objectGenerator.GenerateObjectFromSchema(appJsonResponse.Schema);
                                }
                            }

                            return Results.Ok(response);
                        });
                        break;
                    case OperationType.Put:
                        builder.MapPut(url, () =>
                        {
                            return Results.Ok();
                        });
                        break;
                    case OperationType.Post:
                        builder.MapPost(url, () =>
                        {
                            return Results.Ok();
                        });
                        break;
                    case OperationType.Delete:
                        builder.MapDelete(url, () =>
                        {
                            return Results.Ok();
                        });
                        break;
                    case OperationType.Patch:
                        builder.MapPatch(url, () =>
                        {
                            return Results.Ok();
                        });
                        break;
                    case OperationType.Options:
                    case OperationType.Head:
                    case OperationType.Trace:
                        // TODO: Figure out what to do with these if there is anything
                        break;

                }
            }
        }

        return builder;
    }
}
