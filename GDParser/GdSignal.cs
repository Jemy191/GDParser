using System.Collections.ObjectModel;

namespace GDParser;

public class GdSignal
{
    public readonly string Name;
    public readonly ReadOnlyCollection<GdVariable> Parameters;
    
    public GdSignal(string name, ReadOnlyCollection<GdVariable> parameters)
    {
        Name = name;
        Parameters = parameters;
    }
}