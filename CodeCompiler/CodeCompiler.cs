using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace CodeCompilerNs
{
    public class CodeCompiler
    {
        public CodeCompiler()
        {

        }

        public CodeCompiler(string runtimePath, CSharpCompilationOptions cSharpCompilationOptions, IEnumerable<string> defaultNamespaces)
        {
            RuntimePath = runtimePath;
            DefaultCompilationOptions = cSharpCompilationOptions;
            DefaultNamespaces = defaultNamespaces; ;
        }

        //Propeierties
        private readonly Dictionary<string, string> extensionsDct = new Dictionary<string, string>
        {
           { "DynamicallyLinkedLibrary", ".dll" },
        };
        public static string RuntimePath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\{0}.dll";
        public static IEnumerable<string> DefaultNamespaces = new[]
            {
                "System",
            };

        public static CSharpCompilationOptions DefaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release)
                .WithUsings(DefaultNamespaces);

        public static LanguageVersion LanguageVersion = LanguageVersion.Default;

        private readonly IEnumerable<MetadataReference> defaultReferences = new[]
        {
                MetadataReference.CreateFromFile(string.Format(RuntimePath, "mscorlib")),
                MetadataReference.CreateFromFile(string.Format(RuntimePath, "System")),
                MetadataReference.CreateFromFile(string.Format(RuntimePath, "System.Core"))
        };


        #region Private functions
        private CSharpCompilation CreateCompilation(string fileName, SyntaxTree parsedSyntaxTree)
        {
            return CSharpCompilation.Create(fileName, new SyntaxTree[] { parsedSyntaxTree }, defaultReferences, DefaultCompilationOptions);
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

        private string ReadFileFromPath(string inputPath)
        {
            return File.ReadAllText(inputPath);
        }

        private void PrepareOutputPath(string fileName, string inputPath, ref string outputPath)
        {
            string ext = "";
            fileName = string.IsNullOrEmpty(fileName) ? Path.GetFileNameWithoutExtension(inputPath) : fileName;
            extensionsDct.TryGetValue(DefaultCompilationOptions.OutputKind.ToString(), out ext);
            outputPath = Path.Combine(outputPath, fileName + ext);
        }
        #endregion

        #region Public functions
        public EmitResult CreateAssemblyToPath(string inputPath, string outputPath, string fileName = "")
        {
            string source = ReadFileFromPath(inputPath);
            PrepareOutputPath(fileName, inputPath, ref outputPath);

            SyntaxTree parsedSyntaxTree = ParseCode(source, "");
            CSharpCompilation compilation = CreateCompilation(fileName, parsedSyntaxTree);
            return EmitAssemblyToFile(compilation, outputPath);
        }

        public byte[] CreateAssemblyToMemory(string inputPath, ref EmitResult emitResult)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}