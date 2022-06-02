# CodeCompilerLibrary
Simple library for helping compile code. Used in [Code compiler service](https://github.com/owik100/CoderCompilerWorkerService).

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
