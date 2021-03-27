using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVCFramework
{
    public class ViewEngine : IViewEngine
    {

        public Stopwatch stopWatch { get; set; }
        public string GetHtml(string template, object model)
        {
             this.stopWatch = new Stopwatch();
            stopWatch.Start();
            var cSharpCode = GenerateCSharpCode(template);
            var code = @$"
            using MVCFramework;
            //using System;
            //using System.Collections.Generic;
            //using System.Linq;
            using System.Text;
                namespace AppViewNamespace
                {{
                 public class AppViewCode:IView
                  {{
                         public string GetHtml()
                   {{
                            
                            var html=new StringBuilder();   
                             {cSharpCode}
                            return html.ToString();
                     }}
                        }}     
                    }}  ";

            IView view = GetInstanceFromCode(code, model);
            stopWatch.Stop();
            
            Console.WriteLine("Watch inside VieEngine     " + stopWatch.ElapsedMilliseconds);
            return view.GetHtml();
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            Console.WriteLine("Begin of GetInstasnceFormCode     " + stopWatch.ElapsedMilliseconds);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("AppView")
           .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            
           compilation=compilation.AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
          .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
           compilation= compilation.AddSyntaxTrees(syntaxTree);
           
            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            if (model != null)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
            }
            foreach (var library in libraries)
            {
                compilation=compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }
            Console.WriteLine("Before emit   " + stopWatch.ElapsedMilliseconds);
            using var memoryStream = new MemoryStream();
            var compilationResult = compilation.Emit(memoryStream);
            Console.WriteLine("After emit   " + stopWatch.ElapsedMilliseconds);

            if (!compilationResult.Success)
            {
                Console.WriteLine(
                  compilationResult.Diagnostics
                  .Where(x => x.Severity == DiagnosticSeverity.Error)
                  .Select(x => x.GetMessage()));
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var assemblyByteArray = memoryStream.ToArray();
            var appViewAssembly = Assembly.Load(assemblyByteArray);
             var appViewCodeType = appViewAssembly.GetType("AppViewNamespace.AppViewCode");
            var appViewInstance = Activator.CreateInstance(appViewCodeType) as IView;
            Console.WriteLine("End of GetInstasnceFormCode     " + stopWatch.ElapsedMilliseconds);
            return appViewInstance;


        }

        private string GenerateCSharpCode(string template)
        {
            Console.WriteLine("Begin of GeneretaCSharpCode     " + stopWatch.ElapsedMilliseconds);
            var code = new StringBuilder();
            var reader = new StringReader(template);
            string line;
            var cSharpExpressionRegex = new Regex(@"[^\<\""\s&]+", RegexOptions.Compiled);
            var supportedOpperators = new[] { "if", "for", "foreach", "else" };
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim().StartsWith("{") || line.Trim().StartsWith("}"))
                {
                    code.AppendLine(line);
                }
                else if (supportedOpperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    var index = line.IndexOf("@");
                    line = line.Remove(index, 1);
                    code.AppendLine(line);
                }
                else
                {
                    var currentCSharpLine = new StringBuilder("html.AppendLine(@\"");
                    while (line.Contains("@"))
                    {
                        var atSignLocation = line.IndexOf("@");
                        var before = line.Substring(0, atSignLocation);
                        currentCSharpLine.Append(before.Replace("\"", "\"\"") + "\" + ");
                        var cSharpAndEndOfLine = line.Substring(atSignLocation + 1);
                        var cSharpExpression = cSharpExpressionRegex.Match(cSharpAndEndOfLine);
                        currentCSharpLine.Append(cSharpExpression.Value + " + @\"");
                        var after = cSharpAndEndOfLine.Substring(cSharpExpression.Length);
                        line = after;
                    }
                    currentCSharpLine.Append(line.Replace("\"", "\"\"") + "\");");
                    code.AppendLine(currentCSharpLine.ToString());
                }


            }
            Console.WriteLine("End of Generete CSHarpCOde     " + stopWatch.ElapsedMilliseconds);
            return code.ToString();
        }


    }
}
