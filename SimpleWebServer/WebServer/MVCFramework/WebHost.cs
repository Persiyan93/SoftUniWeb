using HTTP;
using MVCFramework.HttpMethodsAttributes;
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
            RegisterRoutes(routeTable, application);
            application.Configure(routeTable);
            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StratAsync();
        }

        private static void RegisterRoutes(List<Route> routeTable, IMvcApplication application)
        {
           var  types= application.GetType().Assembly.GetTypes().Where(t => (t.IsSubclassOf(typeof(Controller)))) ;
                
            
            foreach (var type in types)
            {
                
                Console.WriteLine("TypeName   " + type.GetType());
                var actions = type.GetMethods()
                    .Where(t => !t.IsConstructor && t.IsPublic && t.DeclaringType == type);
                foreach (var action in actions)
                {
                    string path = "/" + type.Name.Replace("Controller", string.Empty) + "/" + action.Name;
                    HttpMethodType methodType = HttpMethodType.Get;
                    var attribute = action.GetCustomAttributes(true)
                        .FirstOrDefault(x => x.GetType()
                                        .IsSubclassOf(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                    if (attribute != null)
                    {
                        methodType = attribute.Type;
                        if (attribute.Path != null)
                        {
                            path = attribute.Path;
                        }
                    }


                    routeTable.Add(new Route(path, methodType, (HttpRequest request) =>
                          {
                              var controller = Activator.CreateInstance(type) as Controller;
                              return action.Invoke(controller,new [] { request}) as HttpResponse;


                          }));
                    Console.WriteLine(routeTable.Count);

                }
            }

        }
    }
}

