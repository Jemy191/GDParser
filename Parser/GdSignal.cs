using System.Collections.ObjectModel;

namespace Parser;

public record GdSignal(string Name, ReadOnlyCollection<GdVariable> Parameters) {}