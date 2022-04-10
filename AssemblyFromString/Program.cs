// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using CodeCompilerNs;


string fileToCompile = @"C:\Users\owik1\Desktop\DesktopProgram.cs";
string outputPath = @"C:\Users\owik1\Desktop\CompilationOutput";

CodeCompiler compiler = new CodeCompiler();
var res = compiler.CreateAssemblyToPath(fileToCompile, outputPath);

if (res.Success)
{

}

//var source = File.ReadAllText(fileToCompile);
//var parsedSyntaxTree = AssemblyMyClass.Parse(source, "Zbychu", null);


//var compilation
//    = CSharpCompilation.Create("Test.dll", new SyntaxTree[] { parsedSyntaxTree }, AssemblyMyClass.DefaultReferences, AssemblyMyClass.DefaultCompilationOptions);
//try
//{
//    var result = compilation.Emit(outputPath);

//    Console.WriteLine(result.Success ? "Sucess!!" : "Failed");



//    if (result.Success)
//    {
//        Assembly asm = Assembly.LoadFrom(outputPath);
//        Type t = asm.GetType("ConsoleApp10.Program");

//        var methodInfoStatic = t.GetMethod("StaticMethodExample");
//        if (methodInfoStatic == null)
//        {
//            // never throw generic Exception - replace this with some other exception type
//            throw new Exception("No such static method exists.");
//        }

//        object[] staticParameters = new object[1];
//        staticParameters[0] = "Parametry!!";
//        methodInfoStatic.Invoke(null, staticParameters);

//        //Non static method
//        var methodInfo = t.GetMethod("AddNumbers", new Type[] { typeof(int), typeof(int) });
//        if (methodInfo == null)
//        {
//            // never throw generic Exception - replace this with some other exception type
//            throw new Exception("No such method exists.");
//        }

//        var o = Activator.CreateInstance(t, null);

//        object[] parameters = new object[2];
//        parameters[0] = 0;            
//        parameters[1] = 2; 

//        var r = methodInfo.Invoke(o, parameters);
//        Console.WriteLine(r);
//    }

//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex);
//}



class AssemblyMyClass
{
    public static readonly IEnumerable<string> DefaultNamespaces =
        new[]
        {
                "System",
                "System.IO",
                "System.Net",
                "System.Linq",
                "System.Text",
                "System.Text.RegularExpressions",
                "System.Collections.Generic"
        };

    public static string runtimePath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\{0}.dll";

    public static readonly IEnumerable<MetadataReference> DefaultReferences =
        new[]
        {
                MetadataReference.CreateFromFile(string.Format(runtimePath, "mscorlib")),
                MetadataReference.CreateFromFile(string.Format(runtimePath, "System")),
                MetadataReference.CreateFromFile(string.Format(runtimePath, "System.Core"))
        };

    public static readonly CSharpCompilationOptions DefaultCompilationOptions =
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release)
                .WithUsings(DefaultNamespaces);

    public static SyntaxTree Parse(string text, string filename = "", CSharpParseOptions options = null)
    {
        var stringText = SourceText.From(text, Encoding.UTF8);
        return SyntaxFactory.ParseSyntaxTree(stringText, options, filename);
    }
}