using System.Collections.ObjectModel;

namespace Parser;

public record GdFunction(string Name, ReadOnlyCollection<GdVariable> Parameters, GdType ReturnType) { }