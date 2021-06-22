using HTTP;
using InvoiceGenerator.Controllers;
using MVCFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator
{
    public class Startup : IMvcApplication
    {
        public void Configure(List<Route> routesTable)
        {
          
        }

        public void ConfigureServices()
        {
    
            //var db = new InvoiceDbContext();
            //db.Database.EnsureCreated();
        }
    }
}
