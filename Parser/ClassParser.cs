using System.Diagnostics.CodeAnalysis;

namespace GDParser;

public static class ClassParser
{
    public static GdClass? Parse(string source)
    {
        using var reader = new StringReader(source
            .Replace("\r\n", Environment.NewLine)
            .Replace("\n", Environment.NewLine)
            .Replace("\\" + Environment.NewLine, " "));

        GdClass? gdClass = null;

        string? className = null;
        GdType? extend = null;
        List<GdAttribute> classAttributes = new();

        string? line;
        do
        {
            line = reader.ReadLine();

            if (CanIgnoreLine(line))
                continue;

            line = line.Trim();

            gdClass ??= ParseClass(line, ref extend, ref className, classAttributes);

            if (gdClass is null)
                continue;

            if (ParseLineVariable(line, gdClass))
                continue;

            if (ParseFunction(line, gdClass, reader, true))
                continue;

            _ = ParseFunction(line, gdClass, reader);

        } while (line is not null);

        return gdClass;
    }

    static bool CanIgnoreLine([NotNullWhen(false)] string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return true;

        if (line.StartsWith("\t") || line.StartsWith(" ") || line.StartsWith("#"))
            return true;

        return false;
    }

    static GdClass? ParseClass(string line, ref GdType? extend, ref string? className, List<GdAttribute> classAttributes)
    {
        var extendToken = GetToken(line, "extends");
        if (extendToken is not null)
            extend = ParseType(line[extendToken.End..].Trim());

        var classNameToken = GetToken(line, "class_name");
        if (classNameToken is not null)
        {
            if (extendToken is null)
                className = line[classNameToken.End..].Trim();
            else
                className = line[classNameToken.End..extendToken.Start].Trim();
        }

        var foundAttributes = ParseClassAttributes(line, extend, className, classAttributes, extendToken, classNameToken);

        if (foundAttributes && extendToken is null && classNameToken is null)
            return null;

        if (extendToken is null && classNameToken is null && !string.IsNullOrWhiteSpace(line))
            return new(className, extend, classAttributes);// We are in the class body and class name and extend cant be in class body

        return null;
    }
    static bool ParseClassAttributes(string line, GdType? extend, string? className, List<GdAttribute> classAttributes, Token? extendToken, Token? classNameToken)
    {
        var attributes = new List<GdAttribute>();

        if (extend is null && className is null && !string.IsNullOrWhiteSpace(line))
            attributes = ParseAttributes(line).Where(a => a.IsClassAttribute).ToList();

        var classDefinitionStart = 0;
        if (extendToken is not null)
            classDefinitionStart = extendToken.Start;
        if (classNameToken is not null && classNameToken.Start < classDefinitionStart)
            classDefinitionStart = classNameToken.Start;

        if (classDefinitionStart != 0)
            attributes = ParseAttributes(line[..classDefinitionStart]).Where(a => a.IsClassAttribute).ToList();

        classAttributes.AddRange(attributes);
        return attributes.Any();
    }

    static bool ParseFunction(string line, GdClass gdClass, TextReader reader, bool parseSignal = false)
    {
        var funcToken = GetToken(line, parseSignal ? "signal" : "func");
        if (funcToken is null)
            return false;

        while (!line.Contains(')'))
        {
            var newLine = reader.ReadLine();
            if (newLine is null)
                break;
            line += newLine;
        }
        var signature = line[funcToken.End..].Trim();

        var typedReturnToken = GetToken(signature, "->");

        var returnType = GdType.Variant;

        if (typedReturnToken is not null)
            returnType = ParseType(signature[typedReturnToken.End..].Split(':')[0].Trim());

        var openingBracketSplit = signature.Split('(');

        var name = openingBracketSplit[0].Trim();

        var parametersStrings = openingBracketSplit[1]
            .Split(')')[0]
            .Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s));

        var parameters = parametersStrings.Select(ParseVariable).ToList();

        if (parseSignal)
            gdClass.Add(new GdSignal(name, parameters.AsReadOnly()));
        else
            gdClass.Add(new GdFunction(name, parameters.AsReadOnly(), returnType));
        return true;
    }

    static bool ParseLineVariable(string line, GdClass gdClass)
    {
        var varToken = GetToken(line, "var");
        if (varToken is null)
            return false;

        var attributes = ParseAttributes(line[..varToken.Start].Trim());

        var declaration = line[varToken.End..].Split('=')[0].Trim();

        var variable = ParseVariable(declaration.Trim());
        variable.SetAttributes(attributes);
        gdClass.Add(variable);

        return true;
    }
    static List<GdAttribute> ParseAttributes(string line) => line
        .Split('@')
        .Where(a => !string.IsNullOrWhiteSpace(a))
        .Select(a => new GdAttribute(a.Trim()))
        .ToList();

    static GdVariable ParseVariable(string text)
    {
        var declaration = text.Trim().Split(':');

        var name = declaration[0].Trim();

        var type = GdType.Variant;
        if (declaration.Length != 1)
            type = ParseType(declaration[1].Trim());

        return new(name, type);
    }

    static GdType ParseType(string text)
    {
        if (text.StartsWith("Array["))
            return GdType.TypedArray(new GdType(text[6..^1]));

        return new(text);
    }

    static Token? GetToken(string line, string token)
    {
        var start = line.IndexOf($"{token} ", StringComparison.Ordinal);
        if (start == -1)
            return null;
        return new(start, start + token.Length);
    }
}