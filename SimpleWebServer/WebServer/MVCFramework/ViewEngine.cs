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
            var code = @$"Using System;
                using System.Text;
                using System.Linq;
                using System.Collections.Generic;
                using SIS.MvcFramework;
                public class AppViewCode : IView
                  {{
                         public string GetHtml(object model, string user)
                   {{
                            var html=new stringBuilder();
                             {cSharpCode}
                            return html.toString();
                     }}
                    }}  ";

            Iview view = GetInstanceFromCode(code, model);
            return view.GetHtml();
        }

        private Iview GetInstanceFromCode(string code, object model)
        {

            var compilation = CSharpCompilation.Create("AppView", options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            compilation.AddSyntaxTrees(syntaxTree);
            compilation.AddReferences(MetadataReference.CreateFromFile(typeof(Iview).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location));
            var libraries = Assembly.Load(new AssemblyName("netstandart")).GetReferencedAssemblies();
            foreach (var library in libraries)
            {
                compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }
            using var memoryStream = new MemoryStream();
            var Compilationresult = compilation.Emit(memoryStream);
            if (!Compilationresult.Success)
            {

            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var assemblyByteArray = memoryStream.ToArray();
            var appViewAssembly = Assembly.Load(assemblyByteArray);
            var appViewCodeType = appViewAssembly.GetType("AppViewNamespace.AppViewCode");
            var appViewInstance = Activator.CreateInstance(appViewCodeType) as Iview;
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
