namespace GDParser;

public class GdType
{
    public readonly bool IsBuiltIn;
    public readonly GdBuiltInType? BuiltInType;
    public readonly string? TypeString;
    public readonly bool IsPath = false;
    public readonly bool IsArray = false;
    public readonly bool IsTypedArray = false;
    public readonly GdType? ArrayType = null;
    
    internal GdType(string type)
    {
        type = type.Trim('"');
        var matchingType = Enum.GetValues<GdBuiltInType>()
            .Where(e => e.ToString() == type)
            .ToList();

        if (matchingType.Count == 0)
        {
            TypeString = type;
            IsPath = TypeString?.StartsWith("res://") ?? false;
            return;
        }

        BuiltInType = matchingType.Single();
        IsArray = BuiltInType is GdBuiltInType.Array or GdBuiltInType.Dictionary;
        IsBuiltIn = true;
    }

    GdType(GdType type)// Typed array ctor
    {
        BuiltInType = GdBuiltInType.Array;
        IsBuiltIn = true;
        IsArray = true;
        IsTypedArray = true;
        ArrayType = type;
    }

    GdType(GdBuiltInType type)
    {
        BuiltInType = type;
        IsBuiltIn = true;
    }

    internal static GdType Variant => new(GdBuiltInType.Variant);

    public override string? ToString() => TypeString ?? BuiltInType.ToString();
    public static GdType TypedArray(GdType gdType) => new GdType(gdType);
}