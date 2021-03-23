using HTTP;
using HTTP.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var routeTable = new List<Route>();
            routeTable.Add(new Route("/", HttpMethodType.Get, Index));
            routeTable.Add(new Route("/users/Login", HttpMethodType.Get, Login));
            routeTable.Add(new Route("/users/Login", HttpMethodType.Post, DoLogin));
            HttpServer httpServer = new HttpServer(80,routeTable);
           
            httpServer.StratAsync().GetAwaiter().GetResult();

        }
        public static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1>Home page</h1>");
        
        }
        public static HttpResponse Login(HttpRequest request)
        {
            return new HtmlResponse("<h1>Login page</h1>");
          
        }
        public static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse( " < h1 > Success Login </ h1 > ");
           
        }
    }
}
