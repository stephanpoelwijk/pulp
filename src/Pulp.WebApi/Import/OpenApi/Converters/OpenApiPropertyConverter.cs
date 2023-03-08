using Microsoft.OpenApi.Models;
using Pulp.WebApi.Extensions.Generic.Models;

namespace Pulp.WebApi.Import.OpenApi.Converters;

internal static class OpenApiPropertyConverter
{
    internal static Property ToProperty(string name, OpenApiSchema schema)
    {
        var propertyTypeSchema = OpenApiSchemaConverter.ToTypeSchema(schema);
        if (propertyTypeSchema is null)
        {
            throw new Exception($"The property '{name}' has an empty schema which is weird");
        }

        return new Property(propertyTypeSchema, name, schema.Nullable);
    }
}
