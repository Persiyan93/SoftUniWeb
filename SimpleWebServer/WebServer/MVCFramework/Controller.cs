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

            Console.WriteLine(model?.GetType().Name);
            IViewEngine viewEngine = new ViewEngine();
            var layout = File.ReadAllText("Views/Common/layout.html");
            var controllerName = this.GetType().Name.Replace("Controller",string.Empty);
             var html = File.ReadAllText("Views/"+controllerName+"/"+viewName+".html");
            var bodyLayout = layout.Replace("@CurrentPage", html);
            return new HtmlResponse(viewEngine.GetHtml(bodyLayout,model));
    

        }
    }
}
