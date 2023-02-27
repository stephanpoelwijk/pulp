using Bogus;
using Microsoft.OpenApi.Models;

namespace Pulp.WebApi.Extensions.OpenApi;

internal class ObjectGenerator : IObjectGenerator
{
    private static Faker Faker = new Faker();

    public object GenerateObjectFromSchema(OpenApiSchema schema)
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
                    return Faker.Random.UInt();
                }
                else if (schema.Format == "int64")
                {
                    return Faker.Random.ULong();
                }

                throw new Exception($"Strange combination type: '{schema.Type}' && format: '{schema.Format}'");

            case "object":
            case null:
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