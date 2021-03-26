using MVCFramework;
using System;
using System.Threading.Tasks;

namespace InvoiceGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
          await  WebHost.StartAsync(new Startup());
           
        }
    }
}
