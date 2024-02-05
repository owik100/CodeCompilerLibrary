using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;
using System.Text;

namespace CodeCompilerNs
{
    public class CodeCompiler
    {
        public CodeCompiler()
        {

        }

        public CodeCompiler(IEnumerable<PortableExecutableReference> referenceAssemblies)
        {
            ReferenceAssembliesProp = referenceAssemblies;
        }

        public CodeCompiler(CSharpCompilationOptions cSharpCompilationOptions)
        {
            CompOptions = cSharpCompilationOptions;
        }

        public CodeCompiler(IEnumerable<PortableExecutableReference> referenceAssemblies, CSharpCompilationOptions cSharpCompilationOptions)
        {
            ReferenceAssembliesProp = referenceAssemblies;
            CompOptions = cSharpCompilationOptions;
        }

        #region Properties
        private readonly Dictionary<string, string> extensionsDct = new Dictionary<string, string>
        {
           { "DynamicallyLinkedLibrary", ".dll" },
           { "ConsoleApplication", ".exe" },
           { "WindowsRuntimeApplication", ".exe" },
        };

        private static CSharpCompilationOptions CompOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release);

        private static IEnumerable<PortableExecutableReference> ReferenceAssembliesProp = Basic.Reference.Assemblies.Net80.References.All;

        private static LanguageVersion LanguageVersion = LanguageVersion.CSharp12;
        #endregion


        #region Private functions
        private CSharpCompilation CreateCompilation(string fileName, SyntaxTree parsedSyntaxTree)
        {
            return CSharpCompilation.Create(fileName, new SyntaxTree[] { parsedSyntaxTree }, references: ReferenceAssembliesProp, CompOptions);
        }

        private SyntaxTree ParseCode(string fileString, string filePath = "")
        {
            SourceText stringText = SourceText.From(fileString, Encoding.UTF8);
            ParseOptions parseOptions = new CSharpParseOptions(LanguageVersion);
            return SyntaxFactory.ParseSyntaxTree(stringText, parseOptions, filePath);
        }

        private EmitResult EmitAssemblyToFile(CSharpCompilation compilation, string outputPath)
        {
            return compilation.Emit(outputPath);
        }

        private Assembly? EmitAssemblyToMemory(CSharpCompilation compilation, ref EmitResult emitResult)
        {
            using var stream = new MemoryStream();
            emitResult = compilation.Emit(stream);
            if (!emitResult.Success)
                return null;

            stream.Position = 0;
            return Assembly.Load(stream.ToArray());
        }

        private string ReadFileFromPath(string inputPath)
        {
            return File.ReadAllText(inputPath);
        }

        private void PrepareOutputPath(ref string fileName, string inputPath, ref string outputPath)
        {
            string ext = "";
            fileName = string.IsNullOrEmpty(fileName) ? Path.GetFileNameWithoutExtension(inputPath) : fileName;
            extensionsDct.TryGetValue(CompOptions.OutputKind.ToString(), out ext);
            outputPath = Path.Combine(outputPath, fileName + ext);
        }
        #endregion

        #region Public functions
        public EmitResult CreateAssemblyToPath(string inputPath, string outputPath, string fileName = "")
        {
            string source = ReadFileFromPath(inputPath);
            PrepareOutputPath(ref fileName, inputPath, ref outputPath);

            SyntaxTree parsedSyntaxTree = ParseCode(source, "");
            CSharpCompilation compilation = CreateCompilation(fileName, parsedSyntaxTree);
            return EmitAssemblyToFile(compilation, outputPath);
        }

        public SyntaxTree ParseCode(string code)
        {
            ParseOptions parseOptions = new CSharpParseOptions(LanguageVersion);
            return SyntaxFactory.ParseSyntaxTree(code, parseOptions);
        }

        public Assembly? CreateAssemblyToMemory(string code, ref EmitResult emitResult)
        {
            SyntaxTree parsedSyntaxTree = ParseCode(code, "");
            CSharpCompilation compilation = CreateCompilation("TempCompilation", parsedSyntaxTree);
            return EmitAssemblyToMemory(compilation, ref emitResult);
        }

        public Assembly? CreateAssemblyToMemory(SyntaxTree syntaxTree, ref EmitResult emitResult)
        {
            CSharpCompilation compilation = CreateCompilation("TempCompilation", syntaxTree);
            return EmitAssemblyToMemory(compilation, ref emitResult);
        }
        #endregion
    }
}