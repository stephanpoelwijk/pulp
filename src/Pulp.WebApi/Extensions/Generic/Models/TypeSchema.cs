namespace Pulp.WebApi.Extensions.Generic.Models;

public class TypeSchema
{
    public static TypeSchema Integer = new(GenerationType.Int, Enumerable.Empty<Property>(), null);
    public static TypeSchema String = new(GenerationType.String, Enumerable.Empty<Property>(), null);

    public GenerationType Type { get; }
    public TypeSchema? ItemType { get; }
    public ContentGenerationHints? ContentGenerationHints { get; }
    public IEnumerable<Property> Properties { get; } = Enumerable.Empty<Property>();

    public TypeSchema(GenerationType type,
        IEnumerable<Property> properties,
        ContentGenerationHints? contentGenerationHints)
    {
        Type = type;
        Properties = properties;
        ContentGenerationHints = contentGenerationHints;
    }

    public TypeSchema(GenerationType type,
        TypeSchema? itemType,
        IEnumerable<Property> properties,
        ContentGenerationHints? contentGenerationHints)
    {
        Type = type;
        ItemType = itemType;
        Properties = properties;
        ContentGenerationHints = contentGenerationHints;
    }
}
