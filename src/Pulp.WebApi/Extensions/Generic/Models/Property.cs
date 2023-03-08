namespace Pulp.WebApi.Extensions.Generic.Models;

public class Property
{
    public string Name { get; } = default!;
    public TypeSchema TypeSchema { get; } = default!;
    public bool IsOptional { get; }

    public Property(TypeSchema typeSchema,
        string name,
        bool isOptional)
    {
        Name = name;
        TypeSchema = typeSchema;
        IsOptional = isOptional;
    }
}
