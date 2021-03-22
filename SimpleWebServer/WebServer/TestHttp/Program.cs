using HTTP;
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
            HttpServer httpServer = new HttpServer(80);
           
            httpServer.StratAsync().GetAwaiter().GetResult();

        }
        public static HttpResponse Index(HttpRequest request)
        {
            
            var content= "<h1>random page</h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);

            return response;
        }
        public static HttpResponse Login(HttpRequest request)
        {

            var content = "<h1>Login page</h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);

            return response;
        }
        public static HttpResponse DoLogin(HttpRequest request)
        {

            var content = "<h1>Success Login</h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);

            return response;
        }
    }
}
