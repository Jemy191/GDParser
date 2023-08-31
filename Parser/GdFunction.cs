using System.Collections.ObjectModel;

namespace GDParser;

public class GdFunction
{
    public readonly string Name;
    public readonly ReadOnlyCollection<GdVariable> Parameters;
    public readonly GdType ReturnType;
    
    public GdFunction(string name, ReadOnlyCollection<GdVariable> parameters, GdType returnType)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
    }
}