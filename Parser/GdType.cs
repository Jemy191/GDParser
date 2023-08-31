namespace GDParser;

public class GdType
{
    public readonly bool IsBuiltIn;
    public readonly GdBuiltInType? BuiltInType;
    public readonly string? TypeString;
    public readonly bool IsPath = false;

    public GdType(string type)
    {
        type = type.Trim('"');
        var matchingType = Enum.GetValues<GdBuiltInType>()
            .Where(e => e.ToString() == type)
            .ToList();

        if (matchingType.Count != 1)
        {
            TypeString = type;
            IsPath = TypeString?.StartsWith("res://") ?? false;
            return;
        }

        IsBuiltIn = true;
        BuiltInType = matchingType.Single();
    }

    GdType(GdBuiltInType type)
    {
        BuiltInType = type;
        IsBuiltIn = true;
    }

    public static GdType Variant => new(GdBuiltInType.Variant);

    public override string? ToString() => TypeString ?? BuiltInType.ToString();
}