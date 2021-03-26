using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
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


        public string GetHtml(string template, object model)
        {
            var cSharpCode = GenerateCSharpCode(template);
            var code = @$"
            using MVCFramework;
            using System;
            using System.Collections.Generic;
            using System.Linq;
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
            return view.GetHtml();
        }

        private IView GetInstanceFromCode(string code, object model)
        {
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
            using var memoryStream = new MemoryStream();
            var compilationResult = compilation.Emit(memoryStream);
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
            var assemblyTypes = appViewAssembly.GetTypes();
            foreach (var type in assemblyTypes)
            {
                Console.Write(type);
            }
            var appViewCodeType = appViewAssembly.GetType("AppViewNamespace.AppViewCode");
            var appViewInstance = Activator.CreateInstance(appViewCodeType) as IView;
            return appViewInstance;


        }

        private string GenerateCSharpCode(string template)
        {
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
            return code.ToString();
        }


    }
}
