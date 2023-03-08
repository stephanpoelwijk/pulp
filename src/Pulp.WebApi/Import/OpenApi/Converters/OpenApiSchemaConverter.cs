using Microsoft.OpenApi.Models;
using Pulp.WebApi.Extensions.Generic.Models;

namespace Pulp.WebApi.Import.OpenApi.Converters;

internal static class OpenApiSchemaConverter
{
    internal static TypeSchema? ToTypeSchema(OpenApiSchema? schema)
    {
        if (schema is null)
        {
            return null;
        }

        var contentGenerationHints = ToGenerationHints(schema);
        var schemaType = GetSchemaType(schema.Type, schema.Format);
        var properties = schema.Properties?.Select(p => OpenApiPropertyConverter.ToProperty(p.Key, p.Value)) ?? Enumerable.Empty<Property>();

        if (schemaType == GenerationType.Array)
        {
            var itemType = ToTypeSchema(schema.Items);
            return new TypeSchema(schemaType, itemType, properties, contentGenerationHints);
        }

        return new TypeSchema(schemaType, properties, contentGenerationHints);
    }

    private static GenerationType GetSchemaType(string type, string format)
    {
        switch (type)
        {
            case "array": return GenerationType.Array;
            case "boolean": return GenerationType.Boolean;
            case "string":
                {
                    if (format == "date")
                        return GenerationType.Date; // TODO: These should be faking hints
                    else if (format == "date-time")
                        return GenerationType.DateTime;

                    // TODO: Do something with enum

                    return GenerationType.String;
                }
            case "number":
                if (format == "float" || string.IsNullOrEmpty(format))
                {
                    return GenerationType.Float;
                }
                else if (format == "double")
                {
                    return GenerationType.Double;
                }

                throw new Exception($"Strange combination type: '{type}' && format: '{format}'");
            case "integer":
                if (format == "int32" || string.IsNullOrEmpty(format))
                {
                    return GenerationType.Int;
                }
                else if (format == "int64")
                {
                    return GenerationType.Long;
                }
                throw new Exception($"Strange combination type: '{type}' && format: '{format}'");
            case "object":
            case null:
                return GenerationType.Object;
            default: throw new Exception($"Unknown type {type}");

        }
    }

    internal static ContentGenerationHints? ToGenerationHints(OpenApiSchema? schema)
    {
        if (schema is null)
        {
            return null;
        }

        return new ContentGenerationHints(null,
            (int?)schema.Minimum,          // TODO: Consider decimal for these
            (int?)schema.Maximum,
            schema.MinLength,
            schema.MaxLength,
            Enumerable.Empty<string>());
    }
}
