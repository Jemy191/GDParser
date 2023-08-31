using System.Collections.ObjectModel;

namespace GDParser;

public class GdClass
{
    public readonly string? ClassName;
    public readonly GdType? Extend;
    public readonly List<GdAttribute> Attributes;

    readonly List<GdVariable> variables = new();
    readonly List<GdFunction> functions = new();
    readonly List<GdSignal> signals = new();
    
    public ReadOnlyCollection<GdVariable> Variables => variables.AsReadOnly();
    public ReadOnlyCollection<GdFunction> Functions => functions.AsReadOnly();
    public ReadOnlyCollection<GdSignal> Signals => signals.AsReadOnly();
    
    public GdClass(string? className, GdType? extend, List<GdAttribute> attributes)
    {
        ClassName = className;
        Extend = extend;
        Attributes = attributes;
    }

    internal void Add(GdVariable variable) => variables.Add(variable);
    internal void Add(GdSignal signal) => signals.Add(signal);
    internal void Add(GdFunction function) => functions.Add(function);
}