using Bogus;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Pulp.WebApi.Extensions.OpenApi;

public static class OpenApiExtensions
{
    private static Faker Faker = new Faker();

    public static IEndpointRouteBuilder UseOpenApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var configuration = builder.ServiceProvider.GetRequiredService<OpenApiConfiguration>();
        if (!File.Exists(configuration.SpecFileName))
        {
            throw new FileNotFoundException($"Could not find OpenApi spec '{configuration.SpecFileName}'", configuration.SpecFileName);
        }

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
                            object response = null;
                            if (operation.Responses.TryGetValue("200", out var okResponse))
                            {
                                if (okResponse.Content.TryGetValue("application/json", out var appJsonResponse))
                                {
                                    response = GenerateObjectFromSchema(appJsonResponse.Schema);
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

    private static object GenerateObjectFromSchema(OpenApiSchema schema)
    {
        if (schema.Type == "array")
        {
            var result = new List<object>();
            for (int i = 0; i < 3; i++)
            {
                result.Add(GenerateObjectFromSchema(schema.Items));
            }

            return result;
        }

        // https://swagger.io/docs/specification/data-models/data-types/#numbers
        switch (schema.Type)
        {
            case "boolean":
                return Faker.Random.Bool();

            case "string": 
                if (schema.Format == "date")
                {
                    return Faker.Date.Recent().ToString("yyyy-MM-dd");
                }
                else if (schema.Format == "date-time")
                {
                    return Faker.Date.Recent().ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                
                return Faker.Random.String2(5, 100);

            case "number": 
                if (schema.Format == "float" || string.IsNullOrEmpty(schema.Format))
                {
                    return Faker.Random.Float();
                }
                else if (schema.Format == "double")
                {
                    return Faker.Random.Double();
                }

                throw new Exception($"Strange combination type: '{schema.Type}' && format: '{schema.Format}'");
                
            case "integer":
                if (schema.Format == "int32" || string.IsNullOrEmpty(schema.Format))
                {
                    return Faker.Random.Int();
                }
                else if (schema.Format == "int64")
                {
                    return Faker.Random.Long();
                }

                throw new Exception($"Strange combination type: '{schema.Type}' && format: '{schema.Format}'");

            case "object":
                {
                    var responseObject = new Dictionary<string, object>();
                    foreach (var propName in schema.Properties.Keys)
                    {
                        responseObject[propName] = GenerateObjectFromSchema(schema.Properties[propName]);
                    }
                    return responseObject;
                }
            default: throw new Exception($"Unknown type {schema.Type}");
        }
    }
}
