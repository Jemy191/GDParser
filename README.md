# GDParser
This is a simple Godot GDScript v2 parser.
For it is only parsing var, func, signal and class attributes.
This project is mainly to simplify C# -> GDScript interoperability.

#### Get it here -> https://www.nuget.org/packages/GDParser/

### How to use
```csharp
using GDParser;

var source = "GDScript source code";
var gdClass = Parser.Parse()

var className = gdClass.Name;
var variables = gdClass.Variables;
```
### Feel free to ask for feature support, doing a feature Pull Request or if you have question.
