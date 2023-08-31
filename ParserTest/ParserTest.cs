using FluentAssertions;
using Parser;

namespace ParserTest;

[UsesVerify]
public class ParserTest
{
    public static TheoryData<string> GetFiles()
    {
        var data = new TheoryData<string>();

        var scriptPaths = Directory.GetFiles("./")
            .Where(f => f.EndsWith(".gd"))
            .ToList();

        foreach (var path in scriptPaths)
        {
            data.Add(path);
        }
        
        return data;
    }
    
    [Theory]
    [MemberData(nameof(GetFiles))]
    public Task ParseAll(string path)
    {
        var setting = new VerifySettings();
        setting.UseFileName(Path.GetFileName(path));

        var gdClass = ClassParser.Parse(File.ReadAllText(path));

        gdClass.Should().NotBeNull();
        
        return Verify(gdClass, setting);
    }
}