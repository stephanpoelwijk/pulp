using Pulp.WebApi.Extensions.Generic.Models;
using Pulp.WebApi.Import.OpenApi;

namespace Pulp.WebApi.Extensions.Generic;

public static class GenericApiExtensions
{
    internal static IEndpointRouteBuilder UseGenericApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var valueGenerator = builder.ServiceProvider.GetRequiredService<IValueGenerator>();
        var configuration = builder.ServiceProvider.GetRequiredService<EndpointConfiguration>();
        var openApiImporter = builder.ServiceProvider.GetRequiredService<OpenApiImporter>();

        var backendService = openApiImporter.Import(configuration.OpenApiSpecFileName);

        foreach (var endpoint in backendService.Endpoints)
        {
            switch (endpoint.Method.ToUpper())
            {
                case "GET":
                    builder.MapGet(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(valueGenerator, endpoint));
                    break;
                case "PUT":
                    builder.MapPut(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(valueGenerator, endpoint));
                    break;
                case "POST":
                    builder.MapPost(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(valueGenerator, endpoint));
                    break;
                case "DELETE":
                    builder.MapDelete(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(valueGenerator, endpoint));
                    break;
                case "PATCH":
                    builder.MapPatch(endpoint.RoutePattern, () => CreateSuccessResponseForEndpoint(valueGenerator, endpoint));
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

    private static IResult CreateSuccessResponseForEndpoint(IValueGenerator valueGenerator, BackendEndpoint endpoint)
    {
        if (endpoint is StaticEndpoint staticEndpoint)
        {
            return Results.Text(staticEndpoint.ResponseContents, staticEndpoint.ContentType);
        }
        else if (endpoint is DynamicEndpoint dynamicEndpoint)
        {
            object? response = CreateObjectForType(valueGenerator, dynamicEndpoint.ResponseTypeSchema);

            return Results.Ok(response);
        }

        throw new NotImplementedException($"No idea what kind of endpoint '{endpoint.GetType().Name}' is");
    }

    private static object? CreateObjectForType(IValueGenerator valueGenerator, TypeSchema? responseType)
    {
        if (responseType is null)
        {
            return null;
        }

        if (responseType.Type == GenerationType.Object)
        {
            var responseObject = new Dictionary<string, object?>();
            foreach (var property in  responseType.Properties)
            {
                object? value = null;
                switch (property.TypeSchema.Type)
                {
                    case GenerationType.Int:
                        value = valueGenerator.Int(property.TypeSchema.ContentGenerationHints);
                        break;
                    case GenerationType.Long:
                        value = valueGenerator.Long(property.TypeSchema.ContentGenerationHints);
                        break;
                    case GenerationType.String:
                        value = valueGenerator.String(property.TypeSchema.ContentGenerationHints);
                        break;
                    case GenerationType.Object:
                        value = CreateObjectForType(valueGenerator, property.TypeSchema);
                        break;
                }

                responseObject[property.Name] = value;
            }

            return responseObject;
        }
        else if (responseType.Type == GenerationType.Array && responseType.ItemType is not null)
        {
            var result = new List<object>();
            for (int i = 0; i < 3; i++)
            {
                result.Add(CreateObjectForType(valueGenerator, responseType.ItemType));
            }

            return result;
        }

        throw new Exception($"Unknown response type {responseType.Type}");
    }
}
