namespace Pulp.WebApi.Extensions.Generic;

public static class GenericApiExtensions
{
    internal static IEndpointRouteBuilder UseGenericApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var backendService = GetTestService();

        var valueGenerator = builder.ServiceProvider.GetRequiredService<IValueGenerator>();

        foreach (var endpoint in backendService.Endpoints)
        {
            switch (endpoint.Method)
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

    private static IResult CreateSuccessResponseForEndpoint(IValueGenerator valueGenerator, Endpoint endpoint)
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

    private static BackendService GetTestService()
    {
        return new BackendService("test",
            new List<Endpoint>()
            {
                new StaticEndpoint("GET", "/someoranother/tests", "application/json", "{\"Hello\": \"World!\"}"),
                new DynamicEndpoint("GET", "/someoranother/dynamictest", new TypeSchema("object", new List<Property>()
                {
                    new Property(TypeSchema.Integer, "id", false),
                    new Property(TypeSchema.String, "name", false),
                    new Property(new TypeSchema("object", new List<Property>()
                    {
                        new Property(TypeSchema.String, "someStringProp", false)
                    }, null), "someSubProperty", false)
                }, null))
            });
    }

    private static object CreateObjectForType(IValueGenerator valueGenerator, TypeSchema responseType)
    {
        if (responseType.Type == "object")
        {
            var responseObject = new Dictionary<string, object?>();
            foreach (var property in  responseType.Properties)
            {
                object? value = null;
                switch (property.TypeSchema.Type)
                {
                    case "int":
                        value = valueGenerator.Int(null, null);
                        break;
                    case "string":
                        value = valueGenerator.String(null, null);
                        break;
                    case "object":
                        value = CreateObjectForType(valueGenerator, property.TypeSchema);
                        break;
                }

                responseObject[property.Name] = value;
            }

            return responseObject;
        }

        throw new Exception($"Unknown response type {responseType.Type}");
    }
}
