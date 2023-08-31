using System.Collections.ObjectModel;

namespace GDParser;

public record GdSignal(string Name, ReadOnlyCollection<GdVariable> Parameters) {}