# CodeCompilerLibrary
Simple library for helping compile code. Used in [Code compiler service](https://github.com/owik100/CoderCompilerWorkerService).
<br/>
In this library i used [Basic Reference Assemblies](https://github.com/jaredpar/basic-reference-assemblies).

## Examples
Compile .cs file to .dll from specific input path to output path.
```csharp
 CodeCompiler codeCompiler = new CodeCompiler();
 var res = codeCompiler.CreateAssemblyToPath(InputPath, OutputPath);
  if (!res.Success)
  {
      StringBuilder sb = new StringBuilder();
      foreach (var item in res.Diagnostics)
      {
          sb.Append(item.GetMessage());
          sb.Append(Environment.NewLine);
      }
     string errMsg = sb.ToString();
  }

```
Pass params to constructor to chose .NET assembly reference version or compilation output type.
```csharp
IEnumerable<PortableExecutableReference> referencesAssemby = ReferenceAssemblies.NetStandard20;
CSharpCompilationOptions compOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release);

CodeCompiler codeCompiler = new CodeCompiler(referencesAssemby, compOptions);
```

Compile code from string to assembly in memory
```csharp
 CodeCompiler codeCompiler = new CodeCompiler();
 string code = @"namespace HelloWorld
                {
                   public class Hello {         
                        static void Main(string[] args)
                        {
                            System.Console.WriteLine(""Hello World!"");
                            System.Console.ReadKey();
                        }
                    }
                }
                ";
 EmitResult emitR = null;
 Assembly assembly = codeCompiler.CreateAssemblyToMemory(code, ref emitR);
```
