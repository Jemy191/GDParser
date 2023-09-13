namespace GDParser;

public class GdType
{
    public readonly bool IsBuiltIn;
    public readonly GdBuiltInType? BuiltInType;
    public readonly string? TypeString;
    public readonly bool IsPath;
    public readonly bool IsArray;
    public readonly bool IsTypedArray;
    public readonly GdType? ArrayType;
    
    internal GdType(string type)
    {
        type = type.Trim('"');
        var matchingType = Enum.GetValues(typeof(GdBuiltInType))
            .Cast<GdBuiltInType>()
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
    public string? ToCSharpTypeString() => BuiltInType switch
    {
        GdBuiltInType.@bool => "bool",
        GdBuiltInType.@int => "long",
        GdBuiltInType.@float => "double",
        GdBuiltInType.String => "string",
        GdBuiltInType.Object => "Godot.GodotObject",
        GdBuiltInType.Callable => "Godot.Callable",
        GdBuiltInType.Signal => "Godot.Signal",
        GdBuiltInType.Dictionary => "Godot.Collections.Dictionary",
        GdBuiltInType.Array when !IsTypedArray => "Godot.Collections.Array",
        GdBuiltInType.Array when IsTypedArray => $"Godot.Collections.Array<{ArrayType.ToCSharpTypeString()}>",
        GdBuiltInType.PackedByteArray => "byte[]",
        GdBuiltInType.PackedInt32Array => "int[]",
        GdBuiltInType.PackedInt64Array => "long[]",
        GdBuiltInType.PackedFloat32Array => "float[]",
        GdBuiltInType.PackedFloat64Array => "double[]",
        GdBuiltInType.PackedStringArray => "string[]",
        GdBuiltInType.PackedVector2Array => "Godot.Vector2[]",
        GdBuiltInType.PackedVector3Array => "Godot.Vector3[]",
        GdBuiltInType.PackedColorArray => "Godot.Color[]",
        _ => TypeString ?? BuiltInType.ToString()
    };

    public static GdType TypedArray(GdType gdType) => new GdType(gdType);
}