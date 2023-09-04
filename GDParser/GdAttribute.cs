namespace GDParser;

public class GdAttribute
{
    public readonly string Name;
    public readonly bool IsClassAttribute = false;

    public GdAttribute(string name)
    {
        Name = name;

        IsClassAttribute =
            name.Contains("icon") ||
            name.Contains("tool") ||
            name.Contains("static_unload");
    }
}