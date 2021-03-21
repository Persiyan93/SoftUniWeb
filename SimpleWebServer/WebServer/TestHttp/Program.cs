using HTTP;
using System;

namespace TestHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer = new HttpServer(80);
           
            httpServer.StratAsync();

        }
    }
}
