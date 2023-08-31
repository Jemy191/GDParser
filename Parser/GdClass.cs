using System.Collections.ObjectModel;

namespace Parser;

public record GdClass(string? ClassName, GdType? Extend, List<GdAttribute> Attributes)
{
    public ReadOnlyCollection<GdVariable> Variables => variables.AsReadOnly();
    public ReadOnlyCollection<GdFunction> Functions => functions.AsReadOnly();
    public ReadOnlyCollection<GdSignal> Signals => signals.AsReadOnly();

    readonly List<GdVariable> variables = new();
    readonly List<GdFunction> functions = new();
    readonly List<GdSignal> signals = new();

    internal void Add(GdVariable variable) => variables.Add(variable);
    internal void Add(GdSignal signal) => signals.Add(signal);
    internal void Add(GdFunction function) => functions.Add(function);
}