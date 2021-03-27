using HTTP;
using HTTP.Response;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MVCFramework
{
    public abstract class Controller
    {
        protected HttpResponse View(object model=null,[CallerMemberName] string viewName = null)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var layout = File.ReadAllText("Views/Common/layout.html");
            var controllerName = this.GetType().Name.Replace("Controller",string.Empty);
             var html = File.ReadAllText("Views/"+controllerName+"/"+viewName+".html");
            var bodyLayout = layout.Replace("@CurrentPage", html);
            stopWatch.Stop();
            Console.WriteLine("Watch after VieGenerator      " + stopWatch.ElapsedMilliseconds);
            return new HtmlResponse( new ViewEngine().GetHtml(bodyLayout,model));
    

        }
    }
}
