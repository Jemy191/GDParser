using System.Collections.ObjectModel;

namespace GDParser;

public record GdFunction(string Name, ReadOnlyCollection<GdVariable> Parameters, GdType ReturnType) { }