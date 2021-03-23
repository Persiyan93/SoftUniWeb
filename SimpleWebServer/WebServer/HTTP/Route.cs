using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    public class Route
    {
        public Route(string path ,HttpMethodType method,Func<HttpRequest,HttpResponse> action)
        {
            this.Path = path;
            this.MethodType = method;
            this.Action = action;
        }
        public string Path { get; set; }

        public HttpMethodType MethodType { get; set; }
        public Func<HttpRequest,HttpResponse> Action { get; set; }
    }
}
