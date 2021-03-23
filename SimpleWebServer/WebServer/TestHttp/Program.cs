using HTTP;
using HTTP.Response;
using System;
using System.Collections.Generic;
using System.IO;
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
            routeTable.Add(new Route("/favicon.ico", HttpMethodType.Get, GetFavIcon));
            HttpServer httpServer = new HttpServer(80,routeTable);
           
            httpServer.StratAsync().GetAwaiter().GetResult();

        }
        public static HttpResponse GetFavIcon(HttpRequest request)
        {
            var favIconByte = File.ReadAllBytes("wwwroot/favicon.ico");
            
            return new FileResponse(favIconByte, "image / x - icon");
        }
        public static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1>Home page</h1>");
        
        }
        public static HttpResponse Login(HttpRequest request)
        {
            if (!request.SessionData.ContainsKey("UserName"))
            {
                request.SessionData ["Username"] = "Pesho";
            }

            var response = new HtmlResponse($"<h1>Hello {request.SessionData["Username"]} page</h1>");
            return response;
          
        }
        public static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse( " < h1 > Success Login </ h1 > ");
           
        }
    }
}
