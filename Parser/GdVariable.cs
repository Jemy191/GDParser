using System.Collections.ObjectModel;

namespace GDParser;

public class GdVariable
{
    public ReadOnlyCollection<GdAttribute> Attributes { get; private set; } = null!;
    public bool IsVariant => Type.BuiltInType == GdBuiltInType.Variant;
    public readonly string Name;
    public readonly GdType Type;
    
    public GdVariable(string name, GdType type)
    {
        Name = name;
        Type = type;
    }

    internal void SetAttributes(List<GdAttribute> attributes) => Attributes = attributes.AsReadOnly();
}