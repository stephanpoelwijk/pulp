namespace Pulp.WebApi.Extensions.Generic;

public static class GenericApiExtensions
{
    internal static IEndpointRouteBuilder UseGenericApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var endpoints = new List<Endpoint>();

        foreach (var endpoint in endpoints)
        {
            switch (endpoint.Method)
            {
                case "GET":
                    builder.MapGet(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(endpoint));
                    break;
                case "PUT":
                    builder.MapPut(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(endpoint));
                    break;
                case "POST":
                    builder.MapPost(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(endpoint));
                    break;
                case "DELETE":
                    builder.MapDelete(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(endpoint));
                    break;
                case "PATCH":
                    builder.MapPatch(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(endpoint));
                    break;
                case "OPTIONS":
                case "HEAD":
                case "TRACE":
                    // TODO: Figure out what to do with these if there is anything that needs to be done
                    break;
            }
        }

        return builder;
    }

    private static IResult CreateSuccessResponseForEndpoint(Endpoint endpoint)
    {
        if (endpoint is StaticEndpoint staticEndpoint)
        {
            return Results.Text(staticEndpoint.ResponseContents, staticEndpoint.ContentType);
        }
        else if (endpoint is DynamicEndpoint dynamicEndpoint)
        {
            object? response = null;

            return Results.Ok(response);
        }

        throw new NotImplementedException($"No idea what kind of endpoint '{endpoint.GetType().Name}' is");
    }
}
