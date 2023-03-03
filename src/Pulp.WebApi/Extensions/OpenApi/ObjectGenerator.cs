using Microsoft.OpenApi.Models;

namespace Pulp.WebApi.Extensions.OpenApi;

internal class ObjectGenerator : IObjectGenerator
{
    private readonly IValueGenerator _valueGenerator;

    public ObjectGenerator(IValueGenerator valueGenerator)
    {
        _valueGenerator = valueGenerator;
    }

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
                return _valueGenerator.Bool();

            case "string":
                if (schema.Format == "date")
                {
                    return _valueGenerator.Date();
                }
                else if (schema.Format == "date-time")
                {
                    return _valueGenerator.DateTime();
                }

                if (schema.Enum != null && schema.Enum.Any())
                {
                    return schema.Enum[_valueGenerator.Int(0, schema.Enum.Count - 1)];
                }

                return _valueGenerator.String(schema.MinLength, schema.MaxLength);

            case "number":
                if (schema.Format == "float" || string.IsNullOrEmpty(schema.Format))
                {
                    return _valueGenerator.Float(schema.Minimum, schema.Maximum);
                }
                else if (schema.Format == "double")
                {
                    return _valueGenerator.Double(schema.Minimum, schema.Maximum);
                }

                throw new Exception($"Strange combination type: '{schema.Type}' && format: '{schema.Format}'");

            case "integer":
                if (schema.Format == "int32" || string.IsNullOrEmpty(schema.Format))
                {
                    return _valueGenerator.Int(schema.Minimum, schema.Maximum);
                }
                else if (schema.Format == "int64")
                {
                    return _valueGenerator.Long(schema.Minimum, schema.Maximum);
                }

                throw new Exception($"Strange combination type: '{schema.Type}' && format: '{schema.Format}'");

            case "object":
            case null:
                {
                    var responseObject = new Dictionary<string, object>();
                    if (schema.Properties != null)
                    {
                        foreach (var propName in schema.Properties.Keys)
                        {
                            responseObject[propName] = GenerateObjectFromSchema(schema.Properties[propName]);
                        }
                    }
                    // TODO: Do something with additionalProperties
                    return responseObject;
                }
            default: throw new Exception($"Unknown type {schema.Type}");
        }
    }
}