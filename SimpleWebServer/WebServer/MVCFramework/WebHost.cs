using HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework
{
    public static class WebHost
    {
        public static async Task StartAsync(IMvcApplication application)
        {
            var routeTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routeTable);

            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StratAsync();
        }
    }
}
