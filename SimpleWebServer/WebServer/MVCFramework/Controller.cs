using HTTP;
using HTTP.Response;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace MVCFramework
{
    public abstract class Controller
    {
        protected HttpResponse View([CallerMemberName] string viewName = null)
        {
            var layout = File.ReadAllText("  ");
            var controllerName = this.GetType().Name.Replace("Controller",string.Empty);
             var html = File.ReadAllText("Views/"+controllerName+"/"+viewName);
            var bodyLayout = layout.Replace("  ",html);
            return new HtmlResponse( bodyLayout);

        }
    }
}
