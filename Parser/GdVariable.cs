using System.Collections.ObjectModel;

namespace Parser;

public record GdVariable(string Name, GdType Type)
{
    public ReadOnlyCollection<GdAttribute> Attributes { get; private set; } = null!;
    public bool IsVariant => Type.BuiltInType == GdBuiltInType.Variant;

    internal void SetAttributes(List<GdAttribute> attributes) => this.Attributes = attributes.AsReadOnly();
}